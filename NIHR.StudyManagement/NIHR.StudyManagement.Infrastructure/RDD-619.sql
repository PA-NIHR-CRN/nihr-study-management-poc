START TRANSACTION;

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

CREATE INDEX `IX_griResearchStudyStatus_GriMappingId` ON `griResearchStudyStatus` (`GriMappingId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240301114842_02-RDD-619', '6.0.25');

COMMIT;

