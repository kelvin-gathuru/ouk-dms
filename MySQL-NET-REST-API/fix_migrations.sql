-- Fix migration history
DELETE FROM __EFMigrationsHistory;
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20211224062302_Initial', '8.0.13');
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20211224062742_Initial_SQL_DATA', '8.0.13');
