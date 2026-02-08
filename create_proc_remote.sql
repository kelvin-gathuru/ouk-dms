DROP PROCEDURE IF EXISTS GetPendingWorkflowTransitions;
DELIMITER //
CREATE PROCEDURE `GetPendingWorkflowTransitions`(
    IN CurrentStepId CHAR(36)
)
BEGIN

    CREATE TEMPORARY TABLE TransitionPaths (
        TransitionId CHAR(36),
        FromStepId CHAR(36),
        ToStepId CHAR(36),
        TransitionName VARCHAR(255),
        TransitionPath TEXT,
        Depth INT
    );

    CREATE TEMPORARY TABLE ProcessQueue (
        TransitionId CHAR(36), 
        FromStepId CHAR(36),
        ToStepId CHAR(36),
        TransitionName VARCHAR(255),
        TransitionPath TEXT,
        Depth INT
    );

    CREATE TEMPORARY TABLE VisitedTransitions (
        TransitionSignature VARCHAR(255)
    );


    INSERT INTO ProcessQueue
    SELECT 
        WT.id AS TransitionId,
        WT.FromStepId,
        WT.ToStepId,
        WT.Name AS TransitionName,
        CONCAT(WT.FromStepId, '->', WT.ToStepId) AS TransitionPath,
        1 AS Depth
    FROM WorkflowTransitions WT
    WHERE WT.FromStepId = CurrentStepId;

    WHILE EXISTS (SELECT 1 FROM ProcessQueue) DO

        INSERT INTO TransitionPaths
        SELECT * FROM ProcessQueue;

        INSERT IGNORE INTO VisitedTransitions (TransitionSignature)
        SELECT CONCAT(FromStepId, '->', ToStepId) FROM ProcessQueue;

        DELETE FROM ProcessQueue;

        INSERT INTO ProcessQueue
        SELECT 
            WT.id AS TransitionId,
            WT.FromStepId,
            WT.ToStepId,
            WT.Name AS TransitionName,
            CONCAT(TP.TransitionPath, ',', WT.FromStepId, '->', WT.ToStepId) AS TransitionPath,
            TP.Depth + 1 AS Depth
        FROM WorkflowTransitions WT
        JOIN TransitionPaths TP ON TP.ToStepId = WT.FromStepId
        LEFT JOIN VisitedTransitions VT ON VT.TransitionSignature = CONCAT(WT.FromStepId, '->', WT.ToStepId)
        WHERE VT.TransitionSignature IS NULL; 
    END WHILE;

    SELECT DISTINCT
        TransitionId,
        FromStepId,
        ToStepId,
        TransitionName
    FROM TransitionPaths
    ORDER BY TransitionId;

    DROP TEMPORARY TABLE IF EXISTS TransitionPaths;
    DROP TEMPORARY TABLE IF EXISTS ProcessQueue;
    DROP TEMPORARY TABLE IF EXISTS VisitedTransitions;
END //
DELIMITER ;
