START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240301114842_02-RDD-619')
BEGIN
    CREATE TABLE `griResearchStudyStatus` (
        `id` int NOT NULL AUTO_INCREMENT,
        `GriMappingId` int NOT NULL,
        `code` varchar(100) NOT NULL,
        `FromDate` datetime(6) NOT NULL,
        `ToDate` datetime(6) NULL,
        `created` datetime(6) NOT NULL,
        PRIMARY KEY (`id`),
        CONSTRAINT `fk_griResearchStudyStatus_griMapping` FOREIGN KEY (`GriMappingId`) REFERENCES `griMapping` (`id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240301114842_02-RDD-619')
BEGIN
    CREATE INDEX `IX_griResearchStudyStatus_GriMappingId` ON `griResearchStudyStatus` (`GriMappingId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240301114842_02-RDD-619')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240301114842_02-RDD-619', '6.0.25');
END;

COMMIT;

