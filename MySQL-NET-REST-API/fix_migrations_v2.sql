-- Fix migration history with CORRECT IDs
DELETE FROM __EFMigrationsHistory;
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20211225124149_Initial', '8.0.13');
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20211225124206_Initial_SQL_Data', '8.0.13');
