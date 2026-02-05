-- Set variables
SET @PrjRoleId = 'e7d1b0c2-233b-4a30-a4c6-585775e55d54';
SET @HodRoleId = '21edbf02-755a-4089-816e-87ea244745f1';

-- Insert new Role
INSERT INTO Roles (Id, Name) VALUES (@HodRoleId, 'HOD');

-- Copy RoleClaims from PRJ to HOD
INSERT INTO RoleClaims (Id, RoleId, ClaimType, ClaimValue, PageActionId)
SELECT UUID(), @HodRoleId, ClaimType, ClaimValue, PageActionId
FROM RoleClaims
WHERE RoleId = @PrjRoleId;

-- Verify insertion
SELECT * FROM Roles WHERE Id = @HodRoleId;
SELECT COUNT(*) AS PermissionCount FROM RoleClaims WHERE RoleId = @HodRoleId;
