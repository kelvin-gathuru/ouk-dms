-- DELETE logic from migration
DELETE FROM RoleClaims
WHERE PageActionId IN (
    '27b7d18e-9fcd-4fe5-a46b-821a8caeac33',
    '11d3365a-95a3-481b-8fb3-cd59dfd2e0c3',
    '5f0aae96-c37e-4753-a1a6-ff05b0d8eb07',
    '33a2e9b6-e1bb-4971-a8b0-121af4f6dd88',
    '6ac2a345-b2f4-4817-b518-dfe302c3a5c4',
    '7c388512-7f43-41f4-a531-9f1ab1903ae8',
    'cbbdde3d-3992-4552-93b1-cdbdf73a3ca0'
);

DELETE FROM PageActions
WHERE Id IN (
    '27b7d18e-9fcd-4fe5-a46b-821a8caeac33',
    '11d3365a-95a3-481b-8fb3-cd59dfd2e0c3',
    '5f0aae96-c37e-4753-a1a6-ff05b0d8eb07',
    '33a2e9b6-e1bb-4971-a8b0-121af4f6dd88',
    '6ac2a345-b2f4-4817-b518-dfe302c3a5c4',
    '7c388512-7f43-41f4-a531-9f1ab1903ae8',
    'cbbdde3d-3992-4552-93b1-cdbdf73a3ca0'
);

-- INSERT logic (omitting Id)
INSERT INTO RoleClaims (RoleId, ClaimType, ClaimValue, PageActionId)
VALUES
    ('fedeac7a-a665-40a4-af02-f47ec4b7aff5', 'Assigned_Edit_Document', '', 'd1f39f95-d550-474b-96fa-99b097b34c1b'),
    ('fedeac7a-a665-40a4-af02-f47ec4b7aff5', 'Assigned_Restore_version', '', '764568df-718a-4a10-a6b5-a11523b22c19'),
    ('fedeac7a-a665-40a4-af02-f47ec4b7aff5', 'Assigned_Send_Email', '', 'e945d35f-d3f6-46e7-9723-a62f89b2022e'),
    ('fedeac7a-a665-40a4-af02-f47ec4b7aff5', 'Assigned_Share_Document', '', '723e56ce-a020-452f-8773-2f27e2725f1b'),
    ('fedeac7a-a665-40a4-af02-f47ec4b7aff5', 'Assigned_Upload_New_version', '', 'dcf6f998-7276-4a1c-aba1-8513f2eaebad');

-- Second block of DELETEs
DELETE RC 
FROM RoleClaims RC
JOIN PageActions PA ON RC.PageActionId = PA.Id
WHERE PA.Code IN ('All_Manage_Indexing', 'Assigned_Manage_Indexing', 'Assigned_Download_Document');

DELETE FROM PageActions
WHERE Code IN ('All_Manage_Indexing', 'Assigned_Manage_Indexing', 'Assigned_Download_Document');

-- Mark migration as applied
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250924100646_Remove_Duplicate_PageActions', '8.0.13');
