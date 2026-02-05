-- Set variables
SET @PrjRoleId = 'e7d1b0c2-233b-4a30-a4c6-585775e55d54';

-- Role IDs
SET @CnaRoleId = '1d199daf-cd7e-4b17-ab86-2a6738b03f5d';
SET @PetitionOfficerRoleId = '18716723-70a1-4430-b0c0-bdf5fc41cf89';
SET @FirstReviewerRoleId = '03730491-9824-4ea0-9306-c4c0a812d803';
SET @DirectorRoleId = 'f7930b98-6756-40bd-b71a-964856c31437';

-- Insert Roles
INSERT INTO Roles (Id, Name) VALUES (@CnaRoleId, 'CNA');
INSERT INTO Roles (Id, Name) VALUES (@PetitionOfficerRoleId, 'Petition Officer');
INSERT INTO Roles (Id, Name) VALUES (@FirstReviewerRoleId, 'First Reviewer(Petitions)');
INSERT INTO Roles (Id, Name) VALUES (@DirectorRoleId, 'Director');

-- Copy RoleClaims from PRJ to CNA
INSERT INTO RoleClaims (Id, RoleId, ClaimType, ClaimValue, PageActionId)
SELECT UUID(), @CnaRoleId, ClaimType, ClaimValue, PageActionId
FROM RoleClaims
WHERE RoleId = @PrjRoleId;

-- Copy RoleClaims from PRJ to Petition Officer
INSERT INTO RoleClaims (Id, RoleId, ClaimType, ClaimValue, PageActionId)
SELECT UUID(), @PetitionOfficerRoleId, ClaimType, ClaimValue, PageActionId
FROM RoleClaims
WHERE RoleId = @PrjRoleId;

-- Copy RoleClaims from PRJ to First Reviewer(Petitions)
INSERT INTO RoleClaims (Id, RoleId, ClaimType, ClaimValue, PageActionId)
SELECT UUID(), @FirstReviewerRoleId, ClaimType, ClaimValue, PageActionId
FROM RoleClaims
WHERE RoleId = @PrjRoleId;

-- Copy RoleClaims from PRJ to Director
INSERT INTO RoleClaims (Id, RoleId, ClaimType, ClaimValue, PageActionId)
SELECT UUID(), @DirectorRoleId, ClaimType, ClaimValue, PageActionId
FROM RoleClaims
WHERE RoleId = @PrjRoleId;

-- Verify insertion
SELECT Id, Name FROM Roles WHERE Id IN (@CnaRoleId, @PetitionOfficerRoleId, @FirstReviewerRoleId, @DirectorRoleId);
SELECT RoleId, COUNT(*) AS PermissionCount FROM RoleClaims WHERE RoleId IN (@CnaRoleId, @PetitionOfficerRoleId, @FirstReviewerRoleId, @DirectorRoleId) GROUP BY RoleId;
