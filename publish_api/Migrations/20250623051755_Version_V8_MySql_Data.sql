INSERT INTO `Users` (`Id`, `FirstName`, `LastName`, `IsDeleted`, `IsSuperAdmin`, `ClientId`, `ClientSecretHash`, `IsSystemUser`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`) VALUES ('6e8b6f4a-9c7d-47b6-a476-b94ac78e2db1', 'System', 'User', 0, 0, NULL, NULL, 1, 'systemuser@gmail.com', 'SYSTEMUSER@GMAIL.COM', 'systemuser@gmail.com', 'SYSTEMUSER@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEM60FYHL5RMKNeB+CxCOI41EC8Vsr1B3Dyrrr2BOtZrxz6doL8o6Tv/tYGDRk20t1A==', 'f3bda79c-c5e4-47ad-bdf5-d8d01c983de2', 'b8f676e1-3e2a-4c3c-b58d-772a9c47e4f9', '1234567890', 0, 0, NULL, 1, 0);

INSERT `Screens` (`Id`, `Name`, `CreatedDate`, `CreatedBy`, `ModifiedDate`, `ModifiedBy`, `DeletedDate`, `DeletedBy`, `IsDeleted`, `OrderNo`) VALUES 
('9e58a819-05ce-4226-ad70-31d2897f94be', 'Archive Retention Period', CAST('2024-12-23T06:59:21.6608642' AS DateTime(6)), '4b352b37-332a-40c6-ab05-e38fcf109719', CAST('2024-12-23T06:59:21.6666667' AS DateTime(6)), '00000000-0000-0000-0000-000000000000', NULL, NULL, 0, NULL);

INSERT `Operations` (`Id`, `Name`, `CreatedDate`, `CreatedBy`, `ModifiedDate`, `ModifiedBy`, `DeletedDate`, `DeletedBy`, `IsDeleted`) VALUES 
('2601bd5f-7987-4cfb-a874-b699a5be3172', 'Manage Archive Retention Period', CAST('2024-12-23T08:22:37.8537805' AS DateTime(6)), '4b352b37-332a-40c6-ab05-e38fcf109719', CAST('2024-12-23T08:22:37.8633333' AS DateTime(6)), '00000000-0000-0000-0000-000000000000', NULL, NULL, 0);

INSERT `ScreenOperations` (`Id`, `OperationId`, `ScreenId`, `CreatedDate`, `CreatedBy`, `ModifiedDate`, `ModifiedBy`, `DeletedDate`, `DeletedBy`, `IsDeleted`) 
	VALUES ('d5e8bac8-c173-459b-be1d-394b2b46f2d9', '2601bd5f-7987-4cfb-a874-b699a5be3172', '9e58a819-05ce-4226-ad70-31d2897f94be', CAST('2024-10-22T16:18:01.0788250' AS DateTime(6)), '00000000-0000-0000-0000-000000000000', CAST('2024-10-22T16:18:01.0788250' AS DateTime(6)), '00000000-0000-0000-0000-000000000000', NULL, '00000000-0000-0000-0000-000000000000', 0);

INSERT `RoleClaims` (`OperationId`, `ScreenId`, `RoleId`, `ClaimType`, `ClaimValue`) VALUES  
('2601bd5f-7987-4cfb-a874-b699a5be3172', '9e58a819-05ce-4226-ad70-31d2897f94be', 'fedeac7a-a665-40a4-af02-f47ec4b7aff5', 'Archive_Retention_Period_Manage_Archive_Retention_Period', '');

Update Screens set OrderNo = 20 where name = 'Archive Retention Period';
Update Screens set OrderNo = 21 where name = 'Document Status';
Update Screens set OrderNo = 22 where name = 'Clients';
Update Screens set OrderNo = 23 where name = 'Role';
Update Screens set OrderNo = 24 where name = 'User';
Update Screens set OrderNo = 25 where name = 'Email';
Update Screens set OrderNo = 26 where name = 'General Settings';
Update Screens set OrderNo = 27 where name = 'Storage Settings';
Update Screens set OrderNo = 28 where name = 'Page Helper';
Update Screens set OrderNo = 29 where name = 'Allow File Extensions';
Update Screens set OrderNo = 30 where name = 'Company Profile';
Update Screens set OrderNo = 31 where name = 'Document Meta Tags';
Update Screens set OrderNo = 32 where name = 'Login Audit';
Update Screens set OrderNo = 33 where name = 'Error Logs';

INSERT `PageHelpers` (`Id`, `Code`, `Name`, `Description`, `CreatedDate`, `CreatedBy`, `ModifiedDate`, `ModifiedBy`, `DeletedDate`, `DeletedBy`, `IsDeleted`) VALUES
(N'0b6c1861-8bf3-413f-980c-1ea483cd5102', N'ARCHIVE_RETENTION_PERIOD', N'Archive Retention Period', N'<p><strong>What is it?</strong><br>Archive Retention Period allows you to automatically move documents to the <strong>delete</strong> after a selected number of days.</p><p><strong>Retention Options:</strong><br>You can choose to automatically delete documents after:</p><p>30 days</p><p>60 days</p><p>90 days</p><p>180 days</p><p>365 days</p><p><strong>How it works:</strong><br>Once this setting is enabled:</p><p>The system will monitor the age of each document.</p><p>When a document reaches the selected retention period (e.g., 30 days), it will be <strong>automatically deleted</strong>.</p><p><i>Enabling this feature helps keep your workspace organized by removing old documents automatically.</i></p>', CAST(N'2023-06-02T17:31:21.0000000' AS DateTime(6)), N'4b352b37-332a-40c6-ab05-e38fcf109719', CAST(N'2025-01-25T10:54:24.7312542' AS DateTime(6)), N'4b352b37-332a-40c6-ab05-e38fcf109719', NULL, NULL, 0);
