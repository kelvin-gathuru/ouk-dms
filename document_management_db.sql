-- MySQL dump 10.13  Distrib 8.0.44, for Linux (x86_64)
--
-- Host: localhost    Database: document_management_db
-- ------------------------------------------------------
-- Server version	8.0.44-0ubuntu0.24.04.1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `AIPromptTemplates`
--

DROP TABLE IF EXISTS `AIPromptTemplates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AIPromptTemplates` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `PromptInput` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsActive` tinyint(1) NOT NULL,
  `ModifiedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AIPromptTemplates`
--

LOCK TABLES `AIPromptTemplates` WRITE;
/*!40000 ALTER TABLE `AIPromptTemplates` DISABLE KEYS */;
INSERT INTO `AIPromptTemplates` VALUES ('0e832c07-8a82-4a5b-b415-cc4b466a9056','Generate tags and keywords for youtube video','Generate tags and keywords for youtube video','Generate tags and keywords about **title** for youtube video.',1,'2025-04-24 08:06:48.000000'),('18849032-284e-4ea5-adaf-35ee52e4ddc4','Generate testimonial','Generate testimonial','Generate testimonial for **subject**. Include details about how it helped you and what you like best about it.',1,'2025-04-24 07:57:12.000000'),('1a4e4a31-f197-4e6f-a58a-e599f216f6ce','Generate blog post conclusion','Generate blog post conclusion','Write blog post conclusion about title: **title**. And the description is **description**.',1,'2025-04-24 08:03:29.000000'),('20804416-cb1b-4016-840d-6f6d625ac210','Write Problem Agitate Solution','Write Problem Agitate Solution','Write Problem-Agitate-Solution copy for the **description**.',1,'2025-04-24 07:57:56.000000'),('30d72e36-1ef7-4ba9-8a8d-db119c013157','Generate Google ads headline for product.','Generate Google ads headline for product.','Write Google ads headline product name: **product name**. Description is **description**. Audience is **audience**.',1,'2025-04-24 08:47:44.000000'),('3b28ed3a-88e3-4d04-8537-039202c28977','Write me blog section','Write me blog section','Write me blog section about **description**.',1,'2025-04-24 07:58:20.000000'),('3bbe9346-2d34-4f43-8510-ab0f2b290459','Generate Instagram post caption','Generate Instagram post caption','Write Instagram post caption about **title**.',1,'2025-04-24 08:07:26.000000'),('3bc3216e-f5c2-4e93-ae40-50100b166f65','Post Generator','Generator Post using Open AI.','Write a post about **description**.',1,'2025-04-23 13:51:27.000000'),('6e80ce92-ebad-4fbe-a466-d26273695fc7','Article Generator','Instantly create unique articles on any topic. Boost engagement, improve SEO, and save time.','Generate article about **article title**',1,'2025-04-23 13:36:00.000000'),('783724b2-f4ed-473b-af76-6952724aa880','Generate instagram hastags.','Generate instagram hastags.','Write instagram hastags for **keywords**.',1,'2025-04-24 08:07:57.000000'),('8650d81b-2cf3-4fa3-9123-7426bbbd4d94','Write product description for Product name','Write product description for Product name','Write product description for **product name**.',1,'2025-04-24 07:55:55.000000'),('8985b3bb-c69d-4d3b-a8bc-6baecef2c358','Generate google ads description for product.','Generate google ads description for product.','Write google ads description product name: **product name**. Description is **description**. Audience is **audience**.',1,'2025-04-24 08:49:24.000000'),('8a361cde-138b-4fcd-950b-8e759983a3ac','Grammar Correction','Grammar Correction','Correct the grammar. Text is **description**.',1,'2025-04-24 08:55:42.000000'),('8c288cf3-1ff0-4d40-a98c-2744b954e54f','Generate pros & cons','Generate pros & cons','Generate pros & cons about title:  **title**. Description is **description**.',1,'2025-04-24 08:50:36.000000'),('8c94a143-a07e-4c9d-947c-6a1168c68647','Email Generator','Email Generator','Write email about title: **subject**, description: **description**.',1,'2025-04-24 08:51:56.000000'),('913a8628-b4f2-41e2-a1aa-f44331afcf00','Newsletter Generator','Newsletter Generator','generate newsletter template about product title: **title**, reason: **subject** description: **description**.',1,'2025-04-24 08:53:00.000000'),('98511559-7b1a-42f6-b924-2430a1bdfd5a','Generate Facebook ads title','Generate Facebook ads title','Write Facebook ads title about title: **title**. And description is **description**.',1,'2025-04-24 08:46:58.000000'),('a72ce7d0-720f-48ac-b7f7-7ab25d73a72c','Summarize Text','Summarize Text','Summarize the following text: **text**.',1,'2025-04-23 13:57:10.000000'),('b9e114c7-a2f1-4777-b43a-e36b1e146dbc','FAQ Generator','FAQ Generator','Answer like faq about subject: **title** Description is **description**.',1,'2025-04-24 08:51:34.000000'),('c1804540-d86a-48c6-a321-05f13630f262','Generate website meta description','Generate website meta description','Generate website meta description site name: **title** Description is **description**.',1,'2025-04-24 08:51:04.000000'),('ca26c30b-e537-4c9f-a4b9-ec4cc7b95a1b','Rewrite content','Rewrite content','Rewrite content:  **contents**.',1,'2025-04-24 08:49:45.000000'),('d35d6c5d-9146-464e-bfa9-196f9db0b251','Generate one paragraph','Generate one paragraph','Generate one paragraph about:  **description**. Keywords are **keywords**.',1,'2025-04-24 08:50:11.000000'),('d8d81df2-2859-4c6d-99aa-eb6dabb9cc01','Post Title Generator','Generator a Post Title from Post Description.','Generate Post title about **description**',1,'2025-04-23 13:55:24.000000'),('ddf9b4d8-1ffc-4582-92f7-6e4adc667c95','Generate  company social media post','Generate  company social media post','Write in company social media post, company name: **company name**. About: **description**.',1,'2025-04-24 08:44:33.000000'),('e884ec96-547c-4f81-99e9-40eed842f8b5','Generate youtube video description','Generate youtube video description','write youtube video description about **title**.',1,'2025-04-24 08:05:13.000000'),('ea82c689-ad2a-4b54-b11b-4545af7a236d','Generate YouTube video titles','Generate YouTube video titles','Craft captivating, attention-grabbing video titles about **description** for YouTube rankings.',1,'2025-04-24 08:06:08.000000'),('f3431223-1eba-4f47-b1f5-8a990a3022af','Email Answer Generator','Email Answer Generator','answer this email content: **description**.',1,'2025-04-24 08:52:18.000000'),('f7057b73-0db5-4fe6-bc5e-44cb9e1b35e4','Generate blog post introduction','Generate blog post introduction','Write blog post intro about title: **title**. And the description is **description**.',1,'2025-04-24 08:02:27.000000'),('fd71e2b4-427f-40d9-8ab3-b616fc0cf09b','Generate Facebook ads text','Generate Facebook ads text','Write facebook ads text about title: **title**. And the description is **description**.',1,'2025-04-24 08:04:16.000000'),('fe9b5264-64a2-4772-a033-00088cf11d07','Generate blog post idea','Generate blog post idea','Write blog post article ideas about **description**.',1,'2025-04-24 08:01:51.000000');
/*!40000 ALTER TABLE `AIPromptTemplates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AllowFileExtensions`
--

DROP TABLE IF EXISTS `AllowFileExtensions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AllowFileExtensions` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `FileType` int NOT NULL,
  `Extension` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AllowFileExtensions`
--

LOCK TABLES `AllowFileExtensions` WRITE;
/*!40000 ALTER TABLE `AllowFileExtensions` DISABLE KEYS */;
INSERT INTO `AllowFileExtensions` VALUES ('0AC7C26C-7308-47A3-B11D-E7D3D220F690',8,'json'),('0c0be0a9-0a4e-4f05-8742-3a5d6d74acf0',2,'png,jpg,jpeg,gif,bmp,svg,webp,ico,avif'),('13a28d05-d6be-4e6b-87fe-b784642e2a95',3,'txt'),('3257c50c-a128-4c98-8809-cc2564b7db2a',1,'pdf'),('64dac07d-9072-4661-b537-053a09d42d6e',0,'doc,docx,ppt,pptx,xls,xlsx'),('9eaf6b33-0cef-45a4-bf92-7c525e2ed536',4,'aac,m4a,mp3,ogg,oga,wav'),('ab5db62f-1fc7-49ed-895f-6ac4be6db33a',6,'zip,7z'),('cb1612ef-8e3c-4823-af2b-469f4b0010b8',5,'webm,ogv,mp4'),('D26F0953-14AC-4138-BBB8-94B230FBF5F3',7,'csv');
/*!40000 ALTER TABLE `AllowFileExtensions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ArchiveRetentions`
--

DROP TABLE IF EXISTS `ArchiveRetentions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ArchiveRetentions` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RetentionPeriodInDays` int DEFAULT NULL,
  `IsEnabled` tinyint(1) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ArchiveRetentions`
--

LOCK TABLES `ArchiveRetentions` WRITE;
/*!40000 ALTER TABLE `ArchiveRetentions` DISABLE KEYS */;
/*!40000 ALTER TABLE `ArchiveRetentions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Categories`
--

DROP TABLE IF EXISTS `Categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Categories` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ParentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `ArchiveParentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsArchive` tinyint(1) NOT NULL DEFAULT '0',
  `ArchiveById` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Categories_ParentId` (`ParentId`),
  KEY `IX_Categories_CreatedBy` (`CreatedBy`),
  KEY `IX_Categories_Name_ParentId_IsArchive_IsDeleted` (`Name`,`ParentId`,`IsArchive`,`IsDeleted`),
  KEY `IX_Categories_ArchiveById` (`ArchiveById`),
  CONSTRAINT `FK_Categories_Categories_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `Categories` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Categories_Users_ArchiveById` FOREIGN KEY (`ArchiveById`) REFERENCES `Users` (`Id`),
  CONSTRAINT `FK_Categories_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `Users` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Categories`
--

LOCK TABLES `Categories` WRITE;
/*!40000 ALTER TABLE `Categories` DISABLE KEYS */;
INSERT INTO `Categories` VALUES ('04226dd5-fedc-4fbd-8ba9-c0a5b72c5b39','Resume',NULL,NULL,'2021-12-22 17:12:49','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 17:12:49','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,NULL,0,NULL),('0e628f62-c710-40f2-949d-5b38583869f2','HR Policies 2021','','9cc497f5-1736-4bc6-84a8-316fd983b732','2021-12-22 17:13:48','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 17:13:48','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,NULL,0,NULL),('48c4c825-04d7-44c5-84c8-6d134cb9b36b','Logbooks',NULL,NULL,'2021-12-22 17:12:45','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 17:12:45','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,NULL,0,NULL),('4dbbd372-6acf-4e5d-a1cf-3ca3f7cc190d','HR Policies 2020','','9cc497f5-1736-4bc6-84a8-316fd983b732','2021-12-22 17:13:39','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 17:13:39','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,NULL,0,NULL),('9cc497f5-1736-4bc6-84a8-316fd983b732','HR Policies',NULL,NULL,'2021-12-22 17:13:13','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 17:13:13','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,NULL,0,NULL),('a465e640-4a44-44e9-9821-630cc8da4a4c','Confidential',NULL,NULL,'2021-12-22 17:13:07','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 17:13:07','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,NULL,0,NULL),('ad57c02a-b6cf-4aa3-aad7-9c014c41b3e6','SOP Production',NULL,NULL,'2021-12-22 17:12:57','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 17:12:57','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,NULL,0,NULL),('e6bc300e-6600-442e-b452-9a13213ab980','Quality Assurance Document',NULL,NULL,'2021-12-22 17:13:26','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 17:13:26','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,NULL,0,NULL),('ff37d684-ebd5-4c99-a0c3-af108519a6e0','Petition','Repository for all Petitions ',NULL,'2025-11-27 07:58:50','4b352b37-332a-40c6-ab05-e38fcf109719','2025-11-27 10:58:50','00000000-0000-0000-0000-000000000000',NULL,NULL,0,NULL,0,NULL);
/*!40000 ALTER TABLE `Categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `CategoryRolePermissions`
--

DROP TABLE IF EXISTS `CategoryRolePermissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `CategoryRolePermissions` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CategoryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RoleId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `StartDate` datetime(6) DEFAULT NULL,
  `EndDate` datetime(6) DEFAULT NULL,
  `IsTimeBound` tinyint(1) NOT NULL,
  `IsAllowDownload` tinyint(1) NOT NULL,
  `ParentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_CategoryRolePermissions_CategoryId` (`CategoryId`),
  KEY `IX_CategoryRolePermissions_CreatedBy` (`CreatedBy`),
  KEY `IX_CategoryRolePermissions_RoleId` (`RoleId`),
  CONSTRAINT `FK_CategoryRolePermissions_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`Id`),
  CONSTRAINT `FK_CategoryRolePermissions_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`),
  CONSTRAINT `FK_CategoryRolePermissions_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `CategoryRolePermissions`
--

LOCK TABLES `CategoryRolePermissions` WRITE;
/*!40000 ALTER TABLE `CategoryRolePermissions` DISABLE KEYS */;
/*!40000 ALTER TABLE `CategoryRolePermissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `CategoryUserPermissions`
--

DROP TABLE IF EXISTS `CategoryUserPermissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `CategoryUserPermissions` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CategoryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `StartDate` datetime(6) DEFAULT NULL,
  `EndDate` datetime(6) DEFAULT NULL,
  `IsTimeBound` tinyint(1) NOT NULL,
  `IsAllowDownload` tinyint(1) NOT NULL,
  `ParentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_CategoryUserPermissions_CategoryId` (`CategoryId`),
  KEY `IX_CategoryUserPermissions_CreatedBy` (`CreatedBy`),
  KEY `IX_CategoryUserPermissions_UserId` (`UserId`),
  CONSTRAINT `FK_CategoryUserPermissions_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`Id`),
  CONSTRAINT `FK_CategoryUserPermissions_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `Users` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CategoryUserPermissions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `CategoryUserPermissions`
--

LOCK TABLES `CategoryUserPermissions` WRITE;
/*!40000 ALTER TABLE `CategoryUserPermissions` DISABLE KEYS */;
INSERT INTO `CategoryUserPermissions` VALUES ('61a8c59f-874d-426f-8a72-0ccdd805f403','ff37d684-ebd5-4c99-a0c3-af108519a6e0','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0,0,NULL,'2025-11-27 07:58:50','4b352b37-332a-40c6-ab05-e38fcf109719','0001-01-01 00:00:00','00000000-0000-0000-0000-000000000000',NULL,NULL,0);
/*!40000 ALTER TABLE `CategoryUserPermissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Clients`
--

DROP TABLE IF EXISTS `Clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Clients` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CompanyName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ContactPerson` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Address` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Clients`
--

LOCK TABLES `Clients` WRITE;
/*!40000 ALTER TABLE `Clients` DISABLE KEYS */;
/*!40000 ALTER TABLE `Clients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `CompanyProfiles`
--

DROP TABLE IF EXISTS `CompanyProfiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `CompanyProfiles` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `LogoUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `BannerUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `AllowPdfSignature` tinyint(1) NOT NULL DEFAULT '0',
  `LogoIconUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `OpenAIAPIKey` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `LicenseKey` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `PurchaseCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `GeminiAPIKey` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `CompanyProfiles`
--

LOCK TABLES `CompanyProfiles` WRITE;
/*!40000 ALTER TABLE `CompanyProfiles` DISABLE KEYS */;
INSERT INTO `CompanyProfiles` VALUES ('5bf73f1d-4679-4901-b833-7963b2799f33','Document Mangement','aae7fde7-afbf-485b-b07c-353bda861b6a.jpg','c6213a8e-6157-4882-bd75-bba76808a039.png','2024-10-01 12:39:08','00000000-0000-0000-0000-000000000000','2025-11-27 09:08:50','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0,0,'logo-small.png',NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `CompanyProfiles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `CustomCategories`
--

DROP TABLE IF EXISTS `CustomCategories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `CustomCategories` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ParentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsArchive` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `CustomCategories`
--

LOCK TABLES `CustomCategories` WRITE;
/*!40000 ALTER TABLE `CustomCategories` DISABLE KEYS */;
/*!40000 ALTER TABLE `CustomCategories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DailyReminders`
--

DROP TABLE IF EXISTS `DailyReminders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DailyReminders` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ReminderId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DayOfWeek` int NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DailyReminders_ReminderId` (`ReminderId`),
  CONSTRAINT `FK_DailyReminders_Reminders_ReminderId` FOREIGN KEY (`ReminderId`) REFERENCES `Reminders` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DailyReminders`
--

LOCK TABLES `DailyReminders` WRITE;
/*!40000 ALTER TABLE `DailyReminders` DISABLE KEYS */;
/*!40000 ALTER TABLE `DailyReminders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentAuditTrails`
--

DROP TABLE IF EXISTS `DocumentAuditTrails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentAuditTrails` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `OperationName` int NOT NULL,
  `AssignToUserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `AssignToRoleId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `Comment` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CategoryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DocumentAuditTrails_AssignToRoleId` (`AssignToRoleId`),
  KEY `IX_DocumentAuditTrails_AssignToUserId` (`AssignToUserId`),
  KEY `IX_DocumentAuditTrails_CreatedBy` (`CreatedBy`),
  KEY `IX_DocumentAuditTrails_DocumentId` (`DocumentId`),
  KEY `IX_DocumentAuditTrails_CategoryId` (`CategoryId`),
  CONSTRAINT `FK_DocumentAuditTrails_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`Id`),
  CONSTRAINT `FK_DocumentAuditTrails_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DocumentAuditTrails_Roles_AssignToRoleId` FOREIGN KEY (`AssignToRoleId`) REFERENCES `Roles` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DocumentAuditTrails_Users_AssignToUserId` FOREIGN KEY (`AssignToUserId`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DocumentAuditTrails_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentAuditTrails`
--

LOCK TABLES `DocumentAuditTrails` WRITE;
/*!40000 ALTER TABLE `DocumentAuditTrails` DISABLE KEYS */;
INSERT INTO `DocumentAuditTrails` VALUES ('08de2d8a-cd13-48fe-86c8-3ce7c04fbdd9',NULL,18,NULL,NULL,'2025-11-27 07:58:50','4b352b37-332a-40c6-ab05-e38fcf109719','2025-11-27 10:58:50','00000000-0000-0000-0000-000000000000',NULL,NULL,0,NULL,'ff37d684-ebd5-4c99-a0c3-af108519a6e0'),('08de2d8a-cd1e-49a8-8149-f7e206add92d',NULL,14,'4b352b37-332a-40c6-ab05-e38fcf109719',NULL,'2025-11-27 07:58:50','4b352b37-332a-40c6-ab05-e38fcf109719','2025-11-27 10:58:50','00000000-0000-0000-0000-000000000000',NULL,NULL,0,NULL,'ff37d684-ebd5-4c99-a0c3-af108519a6e0'),('08de2d8a-cd26-4fa0-8fbf-94619bcb087e',NULL,14,'4b352b37-332a-40c6-ab05-e38fcf109719',NULL,'2025-11-27 07:58:50','4b352b37-332a-40c6-ab05-e38fcf109719','2025-11-27 10:58:50','00000000-0000-0000-0000-000000000000',NULL,NULL,0,NULL,'ff37d684-ebd5-4c99-a0c3-af108519a6e0'),('08de2d8c-4bd8-4a1b-8655-ccc9454f0d3f','b6d60735-c577-41ec-be56-ae142648d730',2,NULL,NULL,'2025-11-27 08:09:32','4b352b37-332a-40c6-ab05-e38fcf109719','2025-11-27 11:09:32','00000000-0000-0000-0000-000000000000',NULL,NULL,0,NULL,NULL),('08de2d8d-3759-4128-8f62-69d9ba10c2d4','b6d60735-c577-41ec-be56-ae142648d730',3,NULL,NULL,'2025-11-27 08:16:08','4b352b37-332a-40c6-ab05-e38fcf109719','2025-11-27 11:16:07','00000000-0000-0000-0000-000000000000',NULL,NULL,0,NULL,NULL),('08de2da3-42f4-4ab1-8a81-7f96358686ee','b6d60735-c577-41ec-be56-ae142648d730',1,NULL,NULL,'2025-11-27 10:53:56','4b352b37-332a-40c6-ab05-e38fcf109719','2025-11-27 13:53:56','00000000-0000-0000-0000-000000000000',NULL,NULL,0,NULL,NULL);
/*!40000 ALTER TABLE `DocumentAuditTrails` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentChunks`
--

DROP TABLE IF EXISTS `DocumentChunks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentChunks` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ChunkIndex` int NOT NULL,
  `Url` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Size` bigint NOT NULL,
  `Extension` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `DocumentVersionId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TotalChunk` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_DocumentChunks_DocumentVersionId` (`DocumentVersionId`),
  CONSTRAINT `FK_DocumentChunks_DocumentVersions_DocumentVersionId` FOREIGN KEY (`DocumentVersionId`) REFERENCES `DocumentVersions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentChunks`
--

LOCK TABLES `DocumentChunks` WRITE;
/*!40000 ALTER TABLE `DocumentChunks` DISABLE KEYS */;
/*!40000 ALTER TABLE `DocumentChunks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentComments`
--

DROP TABLE IF EXISTS `DocumentComments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentComments` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Comment` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DocumentComments_CreatedBy` (`CreatedBy`),
  KEY `IX_DocumentComments_DocumentId` (`DocumentId`),
  CONSTRAINT `FK_DocumentComments_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_DocumentComments_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentComments`
--

LOCK TABLES `DocumentComments` WRITE;
/*!40000 ALTER TABLE `DocumentComments` DISABLE KEYS */;
/*!40000 ALTER TABLE `DocumentComments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentIndexes`
--

DROP TABLE IF EXISTS `DocumentIndexes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentIndexes` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `DocumentVersionId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  PRIMARY KEY (`Id`),
  KEY `IX_DocumentIndexes_DocumentVersionId` (`DocumentVersionId`),
  CONSTRAINT `FK_DocumentIndexes_DocumentVersions_DocumentVersionId` FOREIGN KEY (`DocumentVersionId`) REFERENCES `DocumentVersions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentIndexes`
--

LOCK TABLES `DocumentIndexes` WRITE;
/*!40000 ALTER TABLE `DocumentIndexes` DISABLE KEYS */;
/*!40000 ALTER TABLE `DocumentIndexes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentMetaDatas`
--

DROP TABLE IF EXISTS `DocumentMetaDatas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentMetaDatas` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Metatag` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DocumentMetaTagId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `MetaTagDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DocumentMetaDatas_DocumentId` (`DocumentId`),
  KEY `IX_DocumentMetaDatas_DocumentMetaTagId` (`DocumentMetaTagId`),
  CONSTRAINT `FK_DocumentMetaDatas_DocumentMetaTags_DocumentMetaTagId` FOREIGN KEY (`DocumentMetaTagId`) REFERENCES `DocumentMetaTags` (`Id`),
  CONSTRAINT `FK_DocumentMetaDatas_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentMetaDatas`
--

LOCK TABLES `DocumentMetaDatas` WRITE;
/*!40000 ALTER TABLE `DocumentMetaDatas` DISABLE KEYS */;
INSERT INTO `DocumentMetaDatas` VALUES ('08de2d8c-4b78-48aa-8c56-babe3d027821','b6d60735-c577-41ec-be56-ae142648d730','Petition on Sukari Industries','2025-11-27 08:16:07','4b352b37-332a-40c6-ab05-e38fcf109719','0001-01-01 00:00:00','00000000-0000-0000-0000-000000000000',NULL,NULL,0,'932f7788-3ae5-4925-92d7-f75dcbecfda9',NULL),('08de2d8c-4b7e-4925-821b-a4a3ebe0675e','b6d60735-c577-41ec-be56-ae142648d730','CamScanner','2025-11-27 08:16:07','4b352b37-332a-40c6-ab05-e38fcf109719','0001-01-01 00:00:00','00000000-0000-0000-0000-000000000000',NULL,NULL,0,'563c6044-660f-4b01-8377-52620442c65b',NULL);
/*!40000 ALTER TABLE `DocumentMetaDatas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentMetaTags`
--

DROP TABLE IF EXISTS `DocumentMetaTags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentMetaTags` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Type` int NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsEditable` tinyint(1) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentMetaTags`
--

LOCK TABLES `DocumentMetaTags` WRITE;
/*!40000 ALTER TABLE `DocumentMetaTags` DISABLE KEYS */;
INSERT INTO `DocumentMetaTags` VALUES ('563c6044-660f-4b01-8377-52620442c65b',0,'Author',0,'2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0),('932f7788-3ae5-4925-92d7-f75dcbecfda9',0,'Title',0,'2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0),('ce54bad3-48f6-4df7-a198-cf2bd9de4998',1,'Modified',0,'2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0),('f2a72ebe-dc8f-4aba-a116-3251575c7691',1,'Created',0,'2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0);
/*!40000 ALTER TABLE `DocumentMetaTags` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentRolePermissions`
--

DROP TABLE IF EXISTS `DocumentRolePermissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentRolePermissions` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RoleId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `StartDate` datetime(6) DEFAULT NULL,
  `EndDate` datetime(6) DEFAULT NULL,
  `IsTimeBound` tinyint(1) NOT NULL,
  `IsAllowDownload` tinyint(1) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DocumentRolePermissions_CreatedBy` (`CreatedBy`),
  KEY `IX_DocumentRolePermissions_DocumentId` (`DocumentId`),
  KEY `IX_DocumentRolePermissions_RoleId` (`RoleId`),
  CONSTRAINT `FK_DocumentRolePermissions_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DocumentRolePermissions_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DocumentRolePermissions_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentRolePermissions`
--

LOCK TABLES `DocumentRolePermissions` WRITE;
/*!40000 ALTER TABLE `DocumentRolePermissions` DISABLE KEYS */;
/*!40000 ALTER TABLE `DocumentRolePermissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentShareableLinks`
--

DROP TABLE IF EXISTS `DocumentShareableLinks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentShareableLinks` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `LinkExpiryTime` datetime DEFAULT NULL,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `LinkCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsLinkExpired` tinyint(1) NOT NULL,
  `IsAllowDownload` tinyint(1) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DocumentShareableLinks_DocumentId` (`DocumentId`),
  CONSTRAINT `FK_DocumentShareableLinks_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentShareableLinks`
--

LOCK TABLES `DocumentShareableLinks` WRITE;
/*!40000 ALTER TABLE `DocumentShareableLinks` DISABLE KEYS */;
/*!40000 ALTER TABLE `DocumentShareableLinks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentSignatures`
--

DROP TABLE IF EXISTS `DocumentSignatures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentSignatures` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `SignatureUserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `SignatureUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `SignatureDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DocumentSignatures_DocumentId` (`DocumentId`),
  KEY `IX_DocumentSignatures_SignatureUserId` (`SignatureUserId`),
  CONSTRAINT `FK_DocumentSignatures_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_DocumentSignatures_Users_SignatureUserId` FOREIGN KEY (`SignatureUserId`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentSignatures`
--

LOCK TABLES `DocumentSignatures` WRITE;
/*!40000 ALTER TABLE `DocumentSignatures` DISABLE KEYS */;
/*!40000 ALTER TABLE `DocumentSignatures` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentStatuses`
--

DROP TABLE IF EXISTS `DocumentStatuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentStatuses` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ColorCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentStatuses`
--

LOCK TABLES `DocumentStatuses` WRITE;
/*!40000 ALTER TABLE `DocumentStatuses` DISABLE KEYS */;
/*!40000 ALTER TABLE `DocumentStatuses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentTokens`
--

DROP TABLE IF EXISTS `DocumentTokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentTokens` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Token` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreatedDate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentTokens`
--

LOCK TABLES `DocumentTokens` WRITE;
/*!40000 ALTER TABLE `DocumentTokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `DocumentTokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentUserPermissions`
--

DROP TABLE IF EXISTS `DocumentUserPermissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentUserPermissions` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `StartDate` datetime DEFAULT NULL,
  `EndDate` datetime DEFAULT NULL,
  `IsTimeBound` tinyint(1) NOT NULL,
  `IsAllowDownload` tinyint(1) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DocumentUserPermissions_CreatedBy` (`CreatedBy`),
  KEY `IX_DocumentUserPermissions_DocumentId` (`DocumentId`),
  KEY `IX_DocumentUserPermissions_UserId` (`UserId`),
  CONSTRAINT `FK_DocumentUserPermissions_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DocumentUserPermissions_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `Users` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_DocumentUserPermissions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentUserPermissions`
--

LOCK TABLES `DocumentUserPermissions` WRITE;
/*!40000 ALTER TABLE `DocumentUserPermissions` DISABLE KEYS */;
/*!40000 ALTER TABLE `DocumentUserPermissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DocumentVersions`
--

DROP TABLE IF EXISTS `DocumentVersions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DocumentVersions` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `IV` longblob,
  `Key` longblob,
  `Comment` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `SignById` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `SignDate` datetime(6) DEFAULT NULL,
  `Extension` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsAllChunkUploaded` tinyint(1) NOT NULL DEFAULT '0',
  `IsChunk` tinyint(1) NOT NULL DEFAULT '0',
  `IsCurrentVersion` tinyint(1) NOT NULL DEFAULT '0',
  `VersionNumber` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_DocumentVersions_CreatedBy` (`CreatedBy`),
  KEY `IX_DocumentVersions_SignById` (`SignById`),
  KEY `IX_DocumentVersions_DocumentId_Url_CreatedBy` (`DocumentId`,`Url`,`CreatedBy`),
  CONSTRAINT `FK_DocumentVersions_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `Users` (`Id`),
  CONSTRAINT `FK_DocumentVersions_Users_SignById` FOREIGN KEY (`SignById`) REFERENCES `Users` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DocumentVersions`
--

LOCK TABLES `DocumentVersions` WRITE;
/*!40000 ALTER TABLE `DocumentVersions` DISABLE KEYS */;
INSERT INTO `DocumentVersions` VALUES ('08de2d8c-4b7f-4dc2-8efd-94e3a4b9bc76','b6d60735-c577-41ec-be56-ae142648d730','2a6ebadf-5bae-4610-b052-519b45140838.pdf','2025-11-27 08:09:32','4b352b37-332a-40c6-ab05-e38fcf109719','2025-11-27 08:09:32','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'pdf',1,0,1,1);
/*!40000 ALTER TABLE `DocumentVersions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Documents`
--

DROP TABLE IF EXISTS `Documents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Documents` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CategoryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Url` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DocumentStatusId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IV` longblob,
  `Key` longblob,
  `StorageSettingId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `StorageType` int NOT NULL DEFAULT '0',
  `ClientId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `Comment` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsAddedPageIndxing` tinyint(1) NOT NULL DEFAULT '0',
  `IsArchive` tinyint(1) NOT NULL DEFAULT '0',
  `IsSignatureExists` tinyint(1) NOT NULL DEFAULT '0',
  `SignById` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `SignDate` datetime(6) DEFAULT NULL,
  `DocumentNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Extension` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsAllChunkUploaded` tinyint(1) NOT NULL DEFAULT '0',
  `IsChunk` tinyint(1) NOT NULL DEFAULT '0',
  `IsShared` tinyint(1) NOT NULL DEFAULT '0',
  `ArchiveById` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `OnExpiryAction` int DEFAULT NULL,
  `RetentionDate` date DEFAULT NULL,
  `RetentionPeriodInDays` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Documents_Name_CategoryId_IsArchive_IsDeleted` (`Name`,`CategoryId`,`IsArchive`,`IsDeleted`),
  KEY `IX_Documents_CategoryId` (`CategoryId`),
  KEY `IX_Documents_CreatedBy` (`CreatedBy`),
  KEY `IX_Documents_DocumentStatusId` (`DocumentStatusId`),
  KEY `IX_Documents_StorageSettingId` (`StorageSettingId`),
  KEY `IX_Documents_ClientId` (`ClientId`),
  KEY `IX_Documents_SignById` (`SignById`),
  KEY `IX_Documents_ArchiveById` (`ArchiveById`),
  CONSTRAINT `FK_Documents_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Documents_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `Clients` (`Id`),
  CONSTRAINT `FK_Documents_DocumentStatuses_DocumentStatusId` FOREIGN KEY (`DocumentStatusId`) REFERENCES `DocumentStatuses` (`Id`),
  CONSTRAINT `FK_Documents_StorageSettings_StorageSettingId` FOREIGN KEY (`StorageSettingId`) REFERENCES `StorageSettings` (`Id`),
  CONSTRAINT `FK_Documents_Users_ArchiveById` FOREIGN KEY (`ArchiveById`) REFERENCES `Users` (`Id`),
  CONSTRAINT `FK_Documents_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `Users` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Documents_Users_SignById` FOREIGN KEY (`SignById`) REFERENCES `Users` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Documents`
--

LOCK TABLES `Documents` WRITE;
/*!40000 ALTER TABLE `Documents` DISABLE KEYS */;
INSERT INTO `Documents` VALUES ('b6d60735-c577-41ec-be56-ae142648d730','ff37d684-ebd5-4c99-a0c3-af108519a6e0','Petition on Sukari Industries.pdf',NULL,'2a6ebadf-5bae-4610-b052-519b45140838.pdf','2025-11-27 08:09:32','4b352b37-332a-40c6-ab05-e38fcf109719','2025-11-27 08:16:07','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0,NULL,NULL,NULL,'06890479-e463-4f40-a9a0-080c03e3f7a4',2,NULL,NULL,1,0,0,NULL,NULL,'2025-00001','pdf',1,0,1,NULL,0,NULL,NULL);
/*!40000 ALTER TABLE `Documents` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `EmailSMTPSettings`
--

DROP TABLE IF EXISTS `EmailSMTPSettings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `EmailSMTPSettings` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Host` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `UserName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Port` int NOT NULL,
  `IsDefault` tinyint(1) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `EncryptionType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `FromEmail` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `FromName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `EmailSMTPSettings`
--

LOCK TABLES `EmailSMTPSettings` WRITE;
/*!40000 ALTER TABLE `EmailSMTPSettings` DISABLE KEYS */;
/*!40000 ALTER TABLE `EmailSMTPSettings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `FileRequestDocuments`
--

DROP TABLE IF EXISTS `FileRequestDocuments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `FileRequestDocuments` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Url` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `FileRequestId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `FileRequestDocumentStatus` int NOT NULL,
  `ApprovedRejectedDate` datetime(6) DEFAULT NULL,
  `ApprovalOrRjectedById` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `Reason` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_FileRequestDocuments_ApprovalOrRjectedById` (`ApprovalOrRjectedById`),
  KEY `IX_FileRequestDocuments_FileRequestId` (`FileRequestId`),
  CONSTRAINT `FK_FileRequestDocuments_FileRequests_FileRequestId` FOREIGN KEY (`FileRequestId`) REFERENCES `FileRequests` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_FileRequestDocuments_Users_ApprovalOrRjectedById` FOREIGN KEY (`ApprovalOrRjectedById`) REFERENCES `Users` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `FileRequestDocuments`
--

LOCK TABLES `FileRequestDocuments` WRITE;
/*!40000 ALTER TABLE `FileRequestDocuments` DISABLE KEYS */;
/*!40000 ALTER TABLE `FileRequestDocuments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `FileRequests`
--

DROP TABLE IF EXISTS `FileRequests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `FileRequests` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Subject` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `MaxDocument` int DEFAULT NULL,
  `SizeInMb` int DEFAULT NULL,
  `AllowExtension` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `FileRequestStatus` int NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `CreatedById` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `LinkExpiryTime` datetime(6) DEFAULT NULL,
  `IsLinkExpired` tinyint(1) NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_FileRequests_CreatedById` (`CreatedById`),
  CONSTRAINT `FK_FileRequests_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `FileRequests`
--

LOCK TABLES `FileRequests` WRITE;
/*!40000 ALTER TABLE `FileRequests` DISABLE KEYS */;
/*!40000 ALTER TABLE `FileRequests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HalfYearlyReminders`
--

DROP TABLE IF EXISTS `HalfYearlyReminders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HalfYearlyReminders` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ReminderId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Day` int NOT NULL,
  `Month` int NOT NULL,
  `Quarter` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_HalfYearlyReminders_ReminderId` (`ReminderId`),
  CONSTRAINT `FK_HalfYearlyReminders_Reminders_ReminderId` FOREIGN KEY (`ReminderId`) REFERENCES `Reminders` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HalfYearlyReminders`
--

LOCK TABLES `HalfYearlyReminders` WRITE;
/*!40000 ALTER TABLE `HalfYearlyReminders` DISABLE KEYS */;
/*!40000 ALTER TABLE `HalfYearlyReminders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireAggregatedCounter`
--

DROP TABLE IF EXISTS `HangfireAggregatedCounter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireAggregatedCounter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` int NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_HangfireCounterAggregated_Key` (`Key`)
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireAggregatedCounter`
--

LOCK TABLES `HangfireAggregatedCounter` WRITE;
/*!40000 ALTER TABLE `HangfireAggregatedCounter` DISABLE KEYS */;
INSERT INTO `HangfireAggregatedCounter` VALUES (1,'stats:succeeded:2025-11-27',123,'2025-12-27 11:03:42'),(2,'stats:succeeded:2025-11-27-07',17,'2025-11-28 07:58:04'),(3,'stats:succeeded',123,NULL),(8,'stats:succeeded:2025-11-27-08',34,'2025-11-28 08:57:51'),(20,'stats:succeeded:2025-11-27-09',34,'2025-11-28 09:57:38'),(32,'stats:succeeded:2025-11-27-10',33,'2025-11-28 10:59:27'),(43,'stats:succeeded:2025-11-27-11',5,'2025-11-28 11:03:42');
/*!40000 ALTER TABLE `HangfireAggregatedCounter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireCounter`
--

DROP TABLE IF EXISTS `HangfireCounter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireCounter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` int NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_HangfireCounter_Key` (`Key`)
) ENGINE=InnoDB AUTO_INCREMENT=373 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireCounter`
--

LOCK TABLES `HangfireCounter` WRITE;
/*!40000 ALTER TABLE `HangfireCounter` DISABLE KEYS */;
INSERT INTO `HangfireCounter` VALUES (370,'stats:succeeded:2025-11-27',1,'2025-12-27 11:06:12'),(371,'stats:succeeded:2025-11-27-11',1,'2025-11-28 11:06:12'),(372,'stats:succeeded',1,NULL);
/*!40000 ALTER TABLE `HangfireCounter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireDistributedLock`
--

DROP TABLE IF EXISTS `HangfireDistributedLock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireDistributedLock` (
  `Resource` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireDistributedLock`
--

LOCK TABLES `HangfireDistributedLock` WRITE;
/*!40000 ALTER TABLE `HangfireDistributedLock` DISABLE KEYS */;
/*!40000 ALTER TABLE `HangfireDistributedLock` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireHash`
--

DROP TABLE IF EXISTS `HangfireHash`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireHash` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Field` varchar(40) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_HangfireHash_Key_Field` (`Key`,`Field`)
) ENGINE=InnoDB AUTO_INCREMENT=471 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireHash`
--

LOCK TABLES `HangfireHash` WRITE;
/*!40000 ALTER TABLE `HangfireHash` DISABLE KEYS */;
INSERT INTO `HangfireHash` VALUES (1,'recurring-job:DailyReminder','Queue','default',NULL),(2,'recurring-job:DailyReminder','Cron','5 0 * * *',NULL),(3,'recurring-job:DailyReminder','TimeZoneId','Africa/Nairobi',NULL),(4,'recurring-job:DailyReminder','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"DailyReminder\"}',NULL),(5,'recurring-job:DailyReminder','CreatedAt','1764227910118',NULL),(6,'recurring-job:DailyReminder','NextExecution','1764277500000',NULL),(7,'recurring-job:DailyReminder','V','2',NULL),(8,'recurring-job:WeeklyReminder','Queue','default',NULL),(9,'recurring-job:WeeklyReminder','Cron','10 0 * * *',NULL),(10,'recurring-job:WeeklyReminder','TimeZoneId','Africa/Nairobi',NULL),(11,'recurring-job:WeeklyReminder','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"WeeklyReminder\"}',NULL),(12,'recurring-job:WeeklyReminder','CreatedAt','1764227910559',NULL),(13,'recurring-job:WeeklyReminder','NextExecution','1764277800000',NULL),(14,'recurring-job:WeeklyReminder','V','2',NULL),(15,'recurring-job:MonthlyReminder','Queue','default',NULL),(16,'recurring-job:MonthlyReminder','Cron','20 0 * * *',NULL),(17,'recurring-job:MonthlyReminder','TimeZoneId','Africa/Nairobi',NULL),(18,'recurring-job:MonthlyReminder','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"MonthyReminder\"}',NULL),(19,'recurring-job:MonthlyReminder','CreatedAt','1764227910614',NULL),(20,'recurring-job:MonthlyReminder','NextExecution','1764278400000',NULL),(21,'recurring-job:MonthlyReminder','V','2',NULL),(22,'recurring-job:QuarterlyReminder','Queue','default',NULL),(23,'recurring-job:QuarterlyReminder','Cron','30 0 * * *',NULL),(24,'recurring-job:QuarterlyReminder','TimeZoneId','Africa/Nairobi',NULL),(25,'recurring-job:QuarterlyReminder','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"QuarterlyReminder\"}',NULL),(26,'recurring-job:QuarterlyReminder','CreatedAt','1764227910659',NULL),(27,'recurring-job:QuarterlyReminder','NextExecution','1764279000000',NULL),(28,'recurring-job:QuarterlyReminder','V','2',NULL),(29,'recurring-job:HalfYearlyReminder','Queue','default',NULL),(30,'recurring-job:HalfYearlyReminder','Cron','40 0 * * *',NULL),(31,'recurring-job:HalfYearlyReminder','TimeZoneId','Africa/Nairobi',NULL),(32,'recurring-job:HalfYearlyReminder','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"HalfYearlyReminder\"}',NULL),(33,'recurring-job:HalfYearlyReminder','CreatedAt','1764227910704',NULL),(34,'recurring-job:HalfYearlyReminder','NextExecution','1764279600000',NULL),(35,'recurring-job:HalfYearlyReminder','V','2',NULL),(36,'recurring-job:YearlyReminder','Queue','default',NULL),(37,'recurring-job:YearlyReminder','Cron','50 0 * * *',NULL),(38,'recurring-job:YearlyReminder','TimeZoneId','Africa/Nairobi',NULL),(39,'recurring-job:YearlyReminder','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"YearlyReminder\"}',NULL),(40,'recurring-job:YearlyReminder','CreatedAt','1764227910751',NULL),(41,'recurring-job:YearlyReminder','NextExecution','1764280200000',NULL),(42,'recurring-job:YearlyReminder','V','2',NULL),(43,'recurring-job:CustomDateReminder','Queue','default',NULL),(44,'recurring-job:CustomDateReminder','Cron','59 0 * * *',NULL),(45,'recurring-job:CustomDateReminder','TimeZoneId','Africa/Nairobi',NULL),(46,'recurring-job:CustomDateReminder','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"CustomDateReminderSchedule\"}',NULL),(47,'recurring-job:CustomDateReminder','CreatedAt','1764227910796',NULL),(48,'recurring-job:CustomDateReminder','NextExecution','1764280740000',NULL),(49,'recurring-job:CustomDateReminder','V','2',NULL),(50,'recurring-job:ReminderSchedule','Queue','default',NULL),(51,'recurring-job:ReminderSchedule','Cron','*/10 * * * *',NULL),(52,'recurring-job:ReminderSchedule','TimeZoneId','Africa/Nairobi',NULL),(53,'recurring-job:ReminderSchedule','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"ReminderSchedule\"}',NULL),(54,'recurring-job:ReminderSchedule','CreatedAt','1764227910841',NULL),(55,'recurring-job:ReminderSchedule','NextExecution','1764241800000',NULL),(56,'recurring-job:ReminderSchedule','V','2',NULL),(57,'recurring-job:SendEmailScheduler','Queue','default',NULL),(58,'recurring-job:SendEmailScheduler','Cron','*/15 * * * *',NULL),(59,'recurring-job:SendEmailScheduler','TimeZoneId','Africa/Nairobi',NULL),(60,'recurring-job:SendEmailScheduler','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"SendEmailScheduler\"}',NULL),(61,'recurring-job:SendEmailScheduler','CreatedAt','1764227910891',NULL),(62,'recurring-job:SendEmailScheduler','NextExecution','1764242100000',NULL),(63,'recurring-job:SendEmailScheduler','V','2',NULL),(64,'recurring-job:DocumentIndexing','Queue','default',NULL),(65,'recurring-job:DocumentIndexing','Cron','*/15 * * * *',NULL),(66,'recurring-job:DocumentIndexing','TimeZoneId','Africa/Nairobi',NULL),(67,'recurring-job:DocumentIndexing','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"DocumentIndexing\"}',NULL),(68,'recurring-job:DocumentIndexing','CreatedAt','1764227910932',NULL),(69,'recurring-job:DocumentIndexing','NextExecution','1764242100000',NULL),(70,'recurring-job:DocumentIndexing','V','2',NULL),(71,'recurring-job:SendEmailReminderForWorkflowTransition','Queue','default',NULL),(72,'recurring-job:SendEmailReminderForWorkflowTransition','Cron','*/3 * * * *',NULL),(73,'recurring-job:SendEmailReminderForWorkflowTransition','TimeZoneId','Africa/Nairobi',NULL),(74,'recurring-job:SendEmailReminderForWorkflowTransition','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"SendEmailReminderForWorkflowTransition\"}',NULL),(75,'recurring-job:SendEmailReminderForWorkflowTransition','CreatedAt','1764227910975',NULL),(76,'recurring-job:SendEmailReminderForWorkflowTransition','NextExecution','1764241740000',NULL),(77,'recurring-job:SendEmailReminderForWorkflowTransition','V','2',NULL),(78,'recurring-job:PermissionCleanup','Queue','default',NULL),(79,'recurring-job:PermissionCleanup','Cron','59 0 * * *',NULL),(80,'recurring-job:PermissionCleanup','TimeZoneId','Africa/Nairobi',NULL),(81,'recurring-job:PermissionCleanup','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"PermissionCleanupSchedule\"}',NULL),(82,'recurring-job:PermissionCleanup','CreatedAt','1764227911018',NULL),(83,'recurring-job:PermissionCleanup','NextExecution','1764280740000',NULL),(84,'recurring-job:PermissionCleanup','V','2',NULL),(85,'recurring-job:ArchiveRetentionDocumentToDelete','Queue','default',NULL),(86,'recurring-job:ArchiveRetentionDocumentToDelete','Cron','5 1 * * *',NULL),(87,'recurring-job:ArchiveRetentionDocumentToDelete','TimeZoneId','Africa/Nairobi',NULL),(88,'recurring-job:ArchiveRetentionDocumentToDelete','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"ArchiveRetentionDocumentToDelete\"}',NULL),(89,'recurring-job:ArchiveRetentionDocumentToDelete','CreatedAt','1764227911061',NULL),(90,'recurring-job:ArchiveRetentionDocumentToDelete','NextExecution','1764281100000',NULL),(91,'recurring-job:ArchiveRetentionDocumentToDelete','V','2',NULL),(92,'recurring-job:ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction','Queue','default',NULL),(93,'recurring-job:ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction','Cron','5 2 * * *',NULL),(94,'recurring-job:ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction','TimeZoneId','Africa/Nairobi',NULL),(95,'recurring-job:ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction','Job','{\"t\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"m\":\"ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction\"}',NULL),(96,'recurring-job:ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction','CreatedAt','1764227911104',NULL),(97,'recurring-job:ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction','NextExecution','1764284700000',NULL),(98,'recurring-job:ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction','V','2',NULL),(99,'recurring-job:ReminderSchedule','LastExecution','1764241230494',NULL),(101,'recurring-job:ReminderSchedule','LastJobId','119',NULL),(102,'recurring-job:SendEmailReminderForWorkflowTransition','LastExecution','1764241564247',NULL),(104,'recurring-job:SendEmailReminderForWorkflowTransition','LastJobId','124',NULL),(114,'recurring-job:SendEmailScheduler','LastExecution','1764241230494',NULL),(116,'recurring-job:SendEmailScheduler','LastJobId','120',NULL),(117,'recurring-job:DocumentIndexing','LastExecution','1764241230494',NULL),(119,'recurring-job:DocumentIndexing','LastJobId','121',NULL);
/*!40000 ALTER TABLE `HangfireHash` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireJob`
--

DROP TABLE IF EXISTS `HangfireJob`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireJob` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StateId` int DEFAULT NULL,
  `StateName` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `InvocationData` longtext NOT NULL,
  `Arguments` longtext NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `ExpireAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_HangfireJob_StateName` (`StateName`)
) ENGINE=InnoDB AUTO_INCREMENT=125 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireJob`
--

LOCK TABLES `HangfireJob` WRITE;
/*!40000 ALTER TABLE `HangfireJob` DISABLE KEYS */;
INSERT INTO `HangfireJob` VALUES (1,3,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:20:21.064668','2025-11-28 07:20:33.724428'),(2,6,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:21:36.838018','2025-11-28 07:21:47.420368'),(3,9,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:24:28.924228','2025-11-28 07:24:32.229250'),(4,12,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:28:14.627728','2025-11-28 07:28:17.587630'),(5,21,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:30:15.100050','2025-11-28 07:30:17.879918'),(6,24,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:30:15.230544','2025-11-28 07:30:18.551965'),(7,22,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:30:15.362603','2025-11-28 07:30:17.978915'),(8,23,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:30:15.486346','2025-11-28 07:30:18.489976'),(9,29,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:46:33.286098','2025-11-28 07:46:35.934182'),(10,36,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:46:34.375470','2025-11-28 07:46:36.824431'),(11,33,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:46:35.656268','2025-11-28 07:46:36.606647'),(12,35,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:46:36.248199','2025-11-28 07:46:36.708562'),(13,39,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:48:18.569009','2025-11-28 07:48:19.047979'),(14,42,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:50:35.078162','2025-11-28 07:50:36.268136'),(15,45,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:51:35.449101','2025-11-28 07:51:36.440704'),(16,48,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:54:08.607574','2025-11-28 07:54:18.066007'),(17,51,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 07:57:55.107005','2025-11-28 07:58:04.163858'),(18,57,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:00:10.290282','2025-11-28 08:00:19.196314'),(19,63,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:00:10.343561','2025-11-28 08:00:19.538717'),(20,61,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:00:10.397444','2025-11-28 08:00:19.392966'),(21,59,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:00:10.477665','2025-11-28 08:00:19.309338'),(22,66,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:03:03.456221','2025-11-28 08:03:04.424305'),(23,69,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:06:33.744163','2025-11-28 08:06:34.763691'),(24,72,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:09:18.682306','2025-11-28 08:09:19.763456'),(25,75,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:10:49.041828','2025-11-28 08:10:49.650470'),(26,78,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:12:49.220786','2025-11-28 08:12:49.693770'),(27,87,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:15:04.411500','2025-11-28 08:15:05.309188'),(28,84,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:15:04.465610','2025-11-28 08:15:04.875899'),(29,86,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:15:04.534027','2025-11-28 08:15:04.969111'),(30,90,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:18:00.882554','2025-11-28 08:18:04.940103'),(31,93,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:20:04.834505','2025-11-28 08:20:05.223316'),(32,96,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:21:20.215438','2025-11-28 08:21:20.391155'),(33,99,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:24:09.024402','2025-11-28 08:24:20.170280'),(34,102,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:27:54.301417','2025-11-28 08:28:05.201016'),(35,109,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:30:11.322596','2025-11-28 08:30:20.228254'),(36,110,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:30:11.375523','2025-11-28 08:30:20.270485'),(37,113,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:30:11.425348','2025-11-28 08:30:20.336794'),(38,114,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:30:11.477304','2025-11-28 08:30:20.366083'),(39,117,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:33:43.912815','2025-11-28 08:33:50.528762'),(40,120,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:36:29.367872','2025-11-28 08:36:35.462400'),(41,123,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:39:05.237534','2025-11-28 08:39:05.492066'),(42,126,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:40:35.390581','2025-11-28 08:40:35.509733'),(43,129,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:42:50.593583','2025-11-28 08:42:50.720865'),(44,134,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:45:06.901903','2025-11-28 08:45:08.255275'),(45,136,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:45:06.955254','2025-11-28 08:45:08.329737'),(46,138,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:45:07.008309','2025-11-28 08:45:08.427296'),(47,141,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:48:14.974080','2025-11-28 08:48:20.725829'),(48,144,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:50:00.796857','2025-11-28 08:50:05.759168'),(49,147,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:51:16.017659','2025-11-28 08:51:20.810235'),(50,150,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:54:23.988225','2025-11-28 08:54:36.034144'),(51,153,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 08:57:39.848983','2025-11-28 08:57:51.106301'),(52,159,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:00:17.985415','2025-11-28 09:00:21.067754'),(53,161,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:00:18.051662','2025-11-28 09:00:21.493910'),(54,163,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:00:18.117189','2025-11-28 09:00:21.663658'),(55,165,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:00:18.184069','2025-11-28 09:00:22.607566'),(56,168,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:04:03.539283','2025-11-28 09:04:06.489354'),(57,171,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:06:18.794491','2025-11-28 09:06:21.531480'),(58,174,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:09:08.403240','2025-11-28 09:09:08.995049'),(59,177,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:10:38.600235','2025-11-28 09:10:39.103475'),(60,180,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:12:38.875380','2025-11-28 09:12:39.167762'),(61,189,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:15:22.159950','2025-11-28 09:15:23.089200'),(62,187,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:15:22.592376','2025-11-28 09:15:22.852875'),(63,188,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:15:22.668453','2025-11-28 09:15:22.928156'),(64,192,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:19:08.059117','2025-11-28 09:19:08.235573'),(65,196,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:21:08.266854','2025-11-28 09:21:08.608384'),(66,198,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:21:08.324925','2025-11-28 09:21:08.890848'),(67,201,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:24:05.461128','2025-11-28 09:24:06.887041'),(68,204,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:27:35.939226','2025-11-28 09:27:36.962362'),(69,211,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:30:17.585864','2025-11-28 09:30:22.075548'),(70,213,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:30:17.644469','2025-11-28 09:30:22.106876'),(71,215,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:30:17.707072','2025-11-28 09:30:22.175343'),(72,216,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:30:17.763856','2025-11-28 09:30:22.226438'),(73,219,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:34:03.361177','2025-11-28 09:34:07.292577'),(74,222,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:36:03.584193','2025-11-28 09:36:07.410046'),(75,225,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:39:16.586755','2025-11-28 09:39:22.471927'),(76,228,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:40:16.724245','2025-11-28 09:40:22.487140'),(77,231,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:42:31.958087','2025-11-28 09:42:37.776545'),(78,238,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:45:10.359111','2025-11-28 09:45:22.780187'),(79,240,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:45:10.413227','2025-11-28 09:45:22.810881'),(80,239,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:45:10.473636','2025-11-28 09:45:22.800082'),(81,243,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:48:55.832635','2025-11-28 09:49:08.103892'),(82,246,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:50:56.044913','2025-11-28 09:51:07.990375'),(83,249,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:51:15.569839','2025-11-28 09:51:23.026825'),(84,252,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:54:04.004054','2025-11-28 09:54:08.335481'),(85,255,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 09:57:34.755100','2025-11-28 09:57:38.237153'),(86,262,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:00:14.267509','2025-11-28 10:00:23.278061'),(87,263,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:00:14.321015','2025-11-28 10:00:23.304990'),(88,266,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:00:14.382083','2025-11-28 10:00:23.400354'),(89,267,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:00:14.439089','2025-11-28 10:00:23.447467'),(90,270,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:03:44.885629','2025-11-28 10:03:54.081385'),(91,273,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:06:00.576492','2025-11-28 10:06:08.683799'),(92,276,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:09:55.681959','2025-11-28 10:10:09.098334'),(93,279,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:10:10.984240','2025-11-28 10:10:24.109465'),(94,282,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:12:26.578444','2025-11-28 10:12:39.030319'),(95,287,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:15:15.621885','2025-11-28 10:15:24.051448'),(96,290,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:15:15.667904','2025-11-28 10:15:24.144490'),(97,291,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:15:15.721211','2025-11-28 10:15:24.150185'),(98,294,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:18:52.676215','2025-11-28 10:19:08.600976'),(99,297,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:20:52.929441','2025-11-28 10:20:53.865732'),(100,300,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:21:08.968128','2025-11-28 10:21:22.613424'),(101,303,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:25:09.258854','2025-11-28 10:25:22.722768'),(102,306,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:27:24.453693','2025-11-28 10:27:37.688988'),(103,315,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:30:06.410234','2025-11-28 10:30:07.817838'),(104,318,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:30:06.478978','2025-11-28 10:30:07.987818'),(105,317,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:30:06.551848','2025-11-28 10:30:07.955218'),(106,316,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:30:06.612361','2025-11-28 10:30:07.859402'),(107,321,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:33:36.906601','2025-11-28 10:33:37.797158'),(108,324,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:36:09.387724','2025-11-28 10:36:22.908329'),(109,330,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:40:09.706614','2025-11-28 10:40:23.002611'),(110,329,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:40:09.760409','2025-11-28 10:40:22.978627'),(111,333,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:42:09.945588','2025-11-28 10:42:22.989415'),(112,340,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:45:13.160949','2025-11-28 10:45:23.064682'),(113,342,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:45:13.210341','2025-11-28 10:45:23.095575'),(114,341,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:45:13.263757','2025-11-28 10:45:23.076579'),(115,345,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:48:43.564583','2025-11-28 10:48:53.212733'),(116,348,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:50:44.712291','2025-11-28 10:50:53.292419'),(117,351,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:51:39.242420','2025-11-28 10:51:53.976601'),(118,354,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 10:59:11.594658','2025-11-28 10:59:27.413207'),(119,363,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"ReminderSchedule\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 11:00:30.525599','2025-11-28 11:00:41.900617'),(120,364,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailScheduler\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 11:00:30.703605','2025-11-28 11:00:41.916087'),(121,365,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"DocumentIndexing\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 11:00:30.865739','2025-11-28 11:00:41.962236'),(122,366,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 11:00:31.048833','2025-11-28 11:00:42.004068'),(123,369,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 11:03:31.492516','2025-11-28 11:03:41.719710'),(124,372,'Succeeded','{\"Type\":\"DocumentManagement.API.Helpers.JobService, DocumentManagement.API\",\"Method\":\"SendEmailReminderForWorkflowTransition\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}','[]','2025-11-27 11:06:04.256229','2025-11-28 11:06:11.738278');
/*!40000 ALTER TABLE `HangfireJob` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireJobParameter`
--

DROP TABLE IF EXISTS `HangfireJobParameter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireJobParameter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `Name` varchar(40) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_HangfireJobParameter_JobId_Name` (`JobId`,`Name`),
  KEY `FK_HangfireJobParameter_Job` (`JobId`),
  CONSTRAINT `FK_HangfireJobParameter_Job` FOREIGN KEY (`JobId`) REFERENCES `HangfireJob` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=497 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireJobParameter`
--

LOCK TABLES `HangfireJobParameter` WRITE;
/*!40000 ALTER TABLE `HangfireJobParameter` DISABLE KEYS */;
INSERT INTO `HangfireJobParameter` VALUES (1,1,'RecurringJobId','\"ReminderSchedule\"'),(2,1,'Time','1764228020'),(3,1,'CurrentCulture','\"en-US\"'),(4,1,'CurrentUICulture','\"en-US\"'),(5,2,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(6,2,'Time','1764228096'),(7,2,'CurrentCulture','\"en-US\"'),(8,2,'CurrentUICulture','\"en-US\"'),(9,3,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(10,3,'Time','1764228268'),(11,3,'CurrentCulture','\"en-US\"'),(12,3,'CurrentUICulture','\"en-US\"'),(13,4,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(14,4,'Time','1764228494'),(15,4,'CurrentCulture','\"en-US\"'),(16,4,'CurrentUICulture','\"en-US\"'),(17,5,'RecurringJobId','\"ReminderSchedule\"'),(18,5,'Time','1764228615'),(19,5,'CurrentCulture','\"en-US\"'),(20,5,'CurrentUICulture','\"en-US\"'),(21,6,'RecurringJobId','\"SendEmailScheduler\"'),(22,6,'Time','1764228615'),(23,6,'CurrentCulture','\"en-US\"'),(24,6,'CurrentUICulture','\"en-US\"'),(25,7,'RecurringJobId','\"DocumentIndexing\"'),(26,7,'Time','1764228615'),(27,7,'CurrentCulture','\"en-US\"'),(28,7,'CurrentUICulture','\"en-US\"'),(29,8,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(30,8,'Time','1764228615'),(31,8,'CurrentCulture','\"en-US\"'),(32,8,'CurrentUICulture','\"en-US\"'),(33,9,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(34,9,'Time','1764229592'),(35,9,'CurrentCulture','\"en-US\"'),(36,9,'CurrentUICulture','\"en-US\"'),(37,10,'RecurringJobId','\"ReminderSchedule\"'),(38,10,'Time','1764229592'),(39,10,'CurrentCulture','\"en-US\"'),(40,10,'CurrentUICulture','\"en-US\"'),(41,11,'RecurringJobId','\"SendEmailScheduler\"'),(42,11,'Time','1764229592'),(43,11,'CurrentCulture','\"en-US\"'),(44,11,'CurrentUICulture','\"en-US\"'),(45,12,'RecurringJobId','\"DocumentIndexing\"'),(46,12,'Time','1764229592'),(47,12,'CurrentCulture','\"en-US\"'),(48,12,'CurrentUICulture','\"en-US\"'),(49,13,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(50,13,'Time','1764229698'),(51,13,'CurrentCulture','\"en-US\"'),(52,13,'CurrentUICulture','\"en-US\"'),(53,14,'RecurringJobId','\"ReminderSchedule\"'),(54,14,'Time','1764229835'),(55,14,'CurrentCulture','\"en-US\"'),(56,14,'CurrentUICulture','\"en-US\"'),(57,15,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(58,15,'Time','1764229895'),(59,15,'CurrentCulture','\"en-US\"'),(60,15,'CurrentUICulture','\"en-US\"'),(61,16,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(62,16,'Time','1764230048'),(63,16,'CurrentCulture','\"en-US\"'),(64,16,'CurrentUICulture','\"en-US\"'),(65,17,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(66,17,'Time','1764230275'),(67,17,'CurrentCulture','\"en-US\"'),(68,17,'CurrentUICulture','\"en-US\"'),(69,18,'RecurringJobId','\"ReminderSchedule\"'),(70,18,'Time','1764230410'),(71,18,'CurrentCulture','\"en-US\"'),(72,18,'CurrentUICulture','\"en-US\"'),(73,19,'RecurringJobId','\"SendEmailScheduler\"'),(74,19,'Time','1764230410'),(75,19,'CurrentCulture','\"en-US\"'),(76,19,'CurrentUICulture','\"en-US\"'),(77,20,'RecurringJobId','\"DocumentIndexing\"'),(78,20,'Time','1764230410'),(79,20,'CurrentCulture','\"en-US\"'),(80,20,'CurrentUICulture','\"en-US\"'),(81,21,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(82,21,'Time','1764230410'),(83,21,'CurrentCulture','\"en-US\"'),(84,21,'CurrentUICulture','\"en-US\"'),(85,22,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(86,22,'Time','1764230583'),(87,22,'CurrentCulture','\"en-US\"'),(88,22,'CurrentUICulture','\"en-US\"'),(89,23,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(90,23,'Time','1764230793'),(91,23,'CurrentCulture','\"en-US\"'),(92,23,'CurrentUICulture','\"en-US\"'),(93,24,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(94,24,'Time','1764230958'),(95,24,'CurrentCulture','\"en-US\"'),(96,24,'CurrentUICulture','\"en-US\"'),(97,25,'RecurringJobId','\"ReminderSchedule\"'),(98,25,'Time','1764231049'),(99,25,'CurrentCulture','\"en-US\"'),(100,25,'CurrentUICulture','\"en-US\"'),(101,26,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(102,26,'Time','1764231169'),(103,26,'CurrentCulture','\"en-US\"'),(104,26,'CurrentUICulture','\"en-US\"'),(105,27,'RecurringJobId','\"SendEmailScheduler\"'),(106,27,'Time','1764231304'),(107,27,'CurrentCulture','\"en-US\"'),(108,27,'CurrentUICulture','\"en-US\"'),(109,28,'RecurringJobId','\"DocumentIndexing\"'),(110,28,'Time','1764231304'),(111,28,'CurrentCulture','\"en-US\"'),(112,28,'CurrentUICulture','\"en-US\"'),(113,29,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(114,29,'Time','1764231304'),(115,29,'CurrentCulture','\"en-US\"'),(116,29,'CurrentUICulture','\"en-US\"'),(117,30,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(118,30,'Time','1764231480'),(119,30,'CurrentCulture','\"en-US\"'),(120,30,'CurrentUICulture','\"en-US\"'),(121,31,'RecurringJobId','\"ReminderSchedule\"'),(122,31,'Time','1764231604'),(123,31,'CurrentCulture','\"en-US\"'),(124,31,'CurrentUICulture','\"en-US\"'),(125,32,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(126,32,'Time','1764231680'),(127,32,'CurrentCulture','\"en-US\"'),(128,32,'CurrentUICulture','\"en-US\"'),(129,33,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(130,33,'Time','1764231849'),(131,33,'CurrentCulture','\"en-US\"'),(132,33,'CurrentUICulture','\"en-US\"'),(133,34,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(134,34,'Time','1764232074'),(135,34,'CurrentCulture','\"en-US\"'),(136,34,'CurrentUICulture','\"en-US\"'),(137,35,'RecurringJobId','\"ReminderSchedule\"'),(138,35,'Time','1764232211'),(139,35,'CurrentCulture','\"en-US\"'),(140,35,'CurrentUICulture','\"en-US\"'),(141,36,'RecurringJobId','\"SendEmailScheduler\"'),(142,36,'Time','1764232211'),(143,36,'CurrentCulture','\"en-US\"'),(144,36,'CurrentUICulture','\"en-US\"'),(145,37,'RecurringJobId','\"DocumentIndexing\"'),(146,37,'Time','1764232211'),(147,37,'CurrentCulture','\"en-US\"'),(148,37,'CurrentUICulture','\"en-US\"'),(149,38,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(150,38,'Time','1764232211'),(151,38,'CurrentCulture','\"en-US\"'),(152,38,'CurrentUICulture','\"en-US\"'),(153,39,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(154,39,'Time','1764232423'),(155,39,'CurrentCulture','\"en-US\"'),(156,39,'CurrentUICulture','\"en-US\"'),(157,40,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(158,40,'Time','1764232589'),(159,40,'CurrentCulture','\"en-US\"'),(160,40,'CurrentUICulture','\"en-US\"'),(161,41,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(162,41,'Time','1764232745'),(163,41,'CurrentCulture','\"en-US\"'),(164,41,'CurrentUICulture','\"en-US\"'),(165,42,'RecurringJobId','\"ReminderSchedule\"'),(166,42,'Time','1764232835'),(167,42,'CurrentCulture','\"en-US\"'),(168,42,'CurrentUICulture','\"en-US\"'),(169,43,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(170,43,'Time','1764232970'),(171,43,'CurrentCulture','\"en-US\"'),(172,43,'CurrentUICulture','\"en-US\"'),(173,44,'RecurringJobId','\"SendEmailScheduler\"'),(174,44,'Time','1764233106'),(175,44,'CurrentCulture','\"en-US\"'),(176,44,'CurrentUICulture','\"en-US\"'),(177,45,'RecurringJobId','\"DocumentIndexing\"'),(178,45,'Time','1764233106'),(179,45,'CurrentCulture','\"en-US\"'),(180,45,'CurrentUICulture','\"en-US\"'),(181,46,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(182,46,'Time','1764233106'),(183,46,'CurrentCulture','\"en-US\"'),(184,46,'CurrentUICulture','\"en-US\"'),(185,47,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(186,47,'Time','1764233294'),(187,47,'CurrentCulture','\"en-US\"'),(188,47,'CurrentUICulture','\"en-US\"'),(189,48,'RecurringJobId','\"ReminderSchedule\"'),(190,48,'Time','1764233400'),(191,48,'CurrentCulture','\"en-US\"'),(192,48,'CurrentUICulture','\"en-US\"'),(193,49,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(194,49,'Time','1764233475'),(195,49,'CurrentCulture','\"en-US\"'),(196,49,'CurrentUICulture','\"en-US\"'),(197,50,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(198,50,'Time','1764233663'),(199,50,'CurrentCulture','\"en-US\"'),(200,50,'CurrentUICulture','\"en-US\"'),(201,51,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(202,51,'Time','1764233859'),(203,51,'CurrentCulture','\"en-US\"'),(204,51,'CurrentUICulture','\"en-US\"'),(205,52,'RecurringJobId','\"ReminderSchedule\"'),(206,52,'Time','1764234017'),(207,52,'CurrentCulture','\"en-US\"'),(208,52,'CurrentUICulture','\"en-US\"'),(209,53,'RecurringJobId','\"SendEmailScheduler\"'),(210,53,'Time','1764234017'),(211,53,'CurrentCulture','\"en-US\"'),(212,53,'CurrentUICulture','\"en-US\"'),(213,54,'RecurringJobId','\"DocumentIndexing\"'),(214,54,'Time','1764234017'),(215,54,'CurrentCulture','\"en-US\"'),(216,54,'CurrentUICulture','\"en-US\"'),(217,55,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(218,55,'Time','1764234017'),(219,55,'CurrentCulture','\"en-US\"'),(220,55,'CurrentUICulture','\"en-US\"'),(221,56,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(222,56,'Time','1764234243'),(223,56,'CurrentCulture','\"en-US\"'),(224,56,'CurrentUICulture','\"en-US\"'),(225,57,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(226,57,'Time','1764234378'),(227,57,'CurrentCulture','\"en-US\"'),(228,57,'CurrentUICulture','\"en-US\"'),(229,58,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(230,58,'Time','1764234548'),(231,58,'CurrentCulture','\"en-US\"'),(232,58,'CurrentUICulture','\"en-US\"'),(233,59,'RecurringJobId','\"ReminderSchedule\"'),(234,59,'Time','1764234638'),(235,59,'CurrentCulture','\"en-US\"'),(236,59,'CurrentUICulture','\"en-US\"'),(237,60,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(238,60,'Time','1764234758'),(239,60,'CurrentCulture','\"en-US\"'),(240,60,'CurrentUICulture','\"en-US\"'),(241,61,'RecurringJobId','\"SendEmailScheduler\"'),(242,61,'Time','1764234922'),(243,61,'CurrentCulture','\"en-US\"'),(244,61,'CurrentUICulture','\"en-US\"'),(245,62,'RecurringJobId','\"DocumentIndexing\"'),(246,62,'Time','1764234922'),(247,62,'CurrentCulture','\"en-US\"'),(248,62,'CurrentUICulture','\"en-US\"'),(249,63,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(250,63,'Time','1764234922'),(251,63,'CurrentCulture','\"en-US\"'),(252,63,'CurrentUICulture','\"en-US\"'),(253,64,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(254,64,'Time','1764235148'),(255,64,'CurrentCulture','\"en-US\"'),(256,64,'CurrentUICulture','\"en-US\"'),(257,65,'RecurringJobId','\"ReminderSchedule\"'),(258,65,'Time','1764235268'),(259,65,'CurrentCulture','\"en-US\"'),(260,65,'CurrentUICulture','\"en-US\"'),(261,66,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(262,66,'Time','1764235268'),(263,66,'CurrentCulture','\"en-US\"'),(264,66,'CurrentUICulture','\"en-US\"'),(265,67,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(266,67,'Time','1764235445'),(267,67,'CurrentCulture','\"en-US\"'),(268,67,'CurrentUICulture','\"en-US\"'),(269,68,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(270,68,'Time','1764235655'),(271,68,'CurrentCulture','\"en-US\"'),(272,68,'CurrentUICulture','\"en-US\"'),(273,69,'RecurringJobId','\"ReminderSchedule\"'),(274,69,'Time','1764235817'),(275,69,'CurrentCulture','\"en-US\"'),(276,69,'CurrentUICulture','\"en-US\"'),(277,70,'RecurringJobId','\"SendEmailScheduler\"'),(278,70,'Time','1764235817'),(279,70,'CurrentCulture','\"en-US\"'),(280,70,'CurrentUICulture','\"en-US\"'),(281,71,'RecurringJobId','\"DocumentIndexing\"'),(282,71,'Time','1764235817'),(283,71,'CurrentCulture','\"en-US\"'),(284,71,'CurrentUICulture','\"en-US\"'),(285,72,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(286,72,'Time','1764235817'),(287,72,'CurrentCulture','\"en-US\"'),(288,72,'CurrentUICulture','\"en-US\"'),(289,73,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(290,73,'Time','1764236043'),(291,73,'CurrentCulture','\"en-US\"'),(292,73,'CurrentUICulture','\"en-US\"'),(293,74,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(294,74,'Time','1764236163'),(295,74,'CurrentCulture','\"en-US\"'),(296,74,'CurrentUICulture','\"en-US\"'),(297,75,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(298,75,'Time','1764236356'),(299,75,'CurrentCulture','\"en-US\"'),(300,75,'CurrentUICulture','\"en-US\"'),(301,76,'RecurringJobId','\"ReminderSchedule\"'),(302,76,'Time','1764236416'),(303,76,'CurrentCulture','\"en-US\"'),(304,76,'CurrentUICulture','\"en-US\"'),(305,77,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(306,77,'Time','1764236551'),(307,77,'CurrentCulture','\"en-US\"'),(308,77,'CurrentUICulture','\"en-US\"'),(309,78,'RecurringJobId','\"SendEmailScheduler\"'),(310,78,'Time','1764236710'),(311,78,'CurrentCulture','\"en-US\"'),(312,78,'CurrentUICulture','\"en-US\"'),(313,79,'RecurringJobId','\"DocumentIndexing\"'),(314,79,'Time','1764236710'),(315,79,'CurrentCulture','\"en-US\"'),(316,79,'CurrentUICulture','\"en-US\"'),(317,80,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(318,80,'Time','1764236710'),(319,80,'CurrentCulture','\"en-US\"'),(320,80,'CurrentUICulture','\"en-US\"'),(321,81,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(322,81,'Time','1764236935'),(323,81,'CurrentCulture','\"en-US\"'),(324,81,'CurrentUICulture','\"en-US\"'),(325,82,'RecurringJobId','\"ReminderSchedule\"'),(326,82,'Time','1764237056'),(327,82,'CurrentCulture','\"en-US\"'),(328,82,'CurrentUICulture','\"en-US\"'),(329,83,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(330,83,'Time','1764237075'),(331,83,'CurrentCulture','\"en-US\"'),(332,83,'CurrentUICulture','\"en-US\"'),(333,84,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(334,84,'Time','1764237243'),(335,84,'CurrentCulture','\"en-US\"'),(336,84,'CurrentUICulture','\"en-US\"'),(337,85,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(338,85,'Time','1764237454'),(339,85,'CurrentCulture','\"en-US\"'),(340,85,'CurrentUICulture','\"en-US\"'),(341,86,'RecurringJobId','\"ReminderSchedule\"'),(342,86,'Time','1764237614'),(343,86,'CurrentCulture','\"en-US\"'),(344,86,'CurrentUICulture','\"en-US\"'),(345,87,'RecurringJobId','\"SendEmailScheduler\"'),(346,87,'Time','1764237614'),(347,87,'CurrentCulture','\"en-US\"'),(348,87,'CurrentUICulture','\"en-US\"'),(349,88,'RecurringJobId','\"DocumentIndexing\"'),(350,88,'Time','1764237614'),(351,88,'CurrentCulture','\"en-US\"'),(352,88,'CurrentUICulture','\"en-US\"'),(353,89,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(354,89,'Time','1764237614'),(355,89,'CurrentCulture','\"en-US\"'),(356,89,'CurrentUICulture','\"en-US\"'),(357,90,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(358,90,'Time','1764237824'),(359,90,'CurrentCulture','\"en-US\"'),(360,90,'CurrentUICulture','\"en-US\"'),(361,91,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(362,91,'Time','1764237960'),(363,91,'CurrentCulture','\"en-US\"'),(364,91,'CurrentUICulture','\"en-US\"'),(365,92,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(366,92,'Time','1764238195'),(367,92,'CurrentCulture','\"en-US\"'),(368,92,'CurrentUICulture','\"en-US\"'),(369,93,'RecurringJobId','\"ReminderSchedule\"'),(370,93,'Time','1764238210'),(371,93,'CurrentCulture','\"en-US\"'),(372,93,'CurrentUICulture','\"en-US\"'),(373,94,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(374,94,'Time','1764238346'),(375,94,'CurrentCulture','\"en-US\"'),(376,94,'CurrentUICulture','\"en-US\"'),(377,95,'RecurringJobId','\"SendEmailScheduler\"'),(378,95,'Time','1764238515'),(379,95,'CurrentCulture','\"en-US\"'),(380,95,'CurrentUICulture','\"en-US\"'),(381,96,'RecurringJobId','\"DocumentIndexing\"'),(382,96,'Time','1764238515'),(383,96,'CurrentCulture','\"en-US\"'),(384,96,'CurrentUICulture','\"en-US\"'),(385,97,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(386,97,'Time','1764238515'),(387,97,'CurrentCulture','\"en-US\"'),(388,97,'CurrentUICulture','\"en-US\"'),(389,98,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(390,98,'Time','1764238732'),(391,98,'CurrentCulture','\"en-US\"'),(392,98,'CurrentUICulture','\"en-US\"'),(393,99,'RecurringJobId','\"ReminderSchedule\"'),(394,99,'Time','1764238852'),(395,99,'CurrentCulture','\"en-US\"'),(396,99,'CurrentUICulture','\"en-US\"'),(397,100,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(398,100,'Time','1764238868'),(399,100,'CurrentCulture','\"en-US\"'),(400,100,'CurrentUICulture','\"en-US\"'),(401,101,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(402,101,'Time','1764239109'),(403,101,'CurrentCulture','\"en-US\"'),(404,101,'CurrentUICulture','\"en-US\"'),(405,102,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(406,102,'Time','1764239244'),(407,102,'CurrentCulture','\"en-US\"'),(408,102,'CurrentUICulture','\"en-US\"'),(409,103,'RecurringJobId','\"ReminderSchedule\"'),(410,103,'Time','1764239406'),(411,103,'CurrentCulture','\"en-US\"'),(412,103,'CurrentUICulture','\"en-US\"'),(413,104,'RecurringJobId','\"SendEmailScheduler\"'),(414,104,'Time','1764239406'),(415,104,'CurrentCulture','\"en-US\"'),(416,104,'CurrentUICulture','\"en-US\"'),(417,105,'RecurringJobId','\"DocumentIndexing\"'),(418,105,'Time','1764239406'),(419,105,'CurrentCulture','\"en-US\"'),(420,105,'CurrentUICulture','\"en-US\"'),(421,106,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(422,106,'Time','1764239406'),(423,106,'CurrentCulture','\"en-US\"'),(424,106,'CurrentUICulture','\"en-US\"'),(425,107,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(426,107,'Time','1764239616'),(427,107,'CurrentCulture','\"en-US\"'),(428,107,'CurrentUICulture','\"en-US\"'),(429,108,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(430,108,'Time','1764239769'),(431,108,'CurrentCulture','\"en-US\"'),(432,108,'CurrentUICulture','\"en-US\"'),(433,109,'RecurringJobId','\"ReminderSchedule\"'),(434,109,'Time','1764240009'),(435,109,'CurrentCulture','\"en-US\"'),(436,109,'CurrentUICulture','\"en-US\"'),(437,110,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(438,110,'Time','1764240009'),(439,110,'CurrentCulture','\"en-US\"'),(440,110,'CurrentUICulture','\"en-US\"'),(441,111,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(442,111,'Time','1764240129'),(443,111,'CurrentCulture','\"en-US\"'),(444,111,'CurrentUICulture','\"en-US\"'),(445,112,'RecurringJobId','\"SendEmailScheduler\"'),(446,112,'Time','1764240313'),(447,112,'CurrentCulture','\"en-US\"'),(448,112,'CurrentUICulture','\"en-US\"'),(449,113,'RecurringJobId','\"DocumentIndexing\"'),(450,113,'Time','1764240313'),(451,113,'CurrentCulture','\"en-US\"'),(452,113,'CurrentUICulture','\"en-US\"'),(453,114,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(454,114,'Time','1764240313'),(455,114,'CurrentCulture','\"en-US\"'),(456,114,'CurrentUICulture','\"en-US\"'),(457,115,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(458,115,'Time','1764240523'),(459,115,'CurrentCulture','\"en-US\"'),(460,115,'CurrentUICulture','\"en-US\"'),(461,116,'RecurringJobId','\"ReminderSchedule\"'),(462,116,'Time','1764240644'),(463,116,'CurrentCulture','\"en-US\"'),(464,116,'CurrentUICulture','\"en-US\"'),(465,117,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(466,117,'Time','1764240699'),(467,117,'CurrentCulture','\"en-US\"'),(468,117,'CurrentUICulture','\"en-US\"'),(469,118,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(470,118,'Time','1764241151'),(471,118,'CurrentCulture','\"en-US\"'),(472,118,'CurrentUICulture','\"en-US\"'),(473,119,'RecurringJobId','\"ReminderSchedule\"'),(474,119,'Time','1764241230'),(475,119,'CurrentCulture','\"en-US\"'),(476,119,'CurrentUICulture','\"en-US\"'),(477,120,'RecurringJobId','\"SendEmailScheduler\"'),(478,120,'Time','1764241230'),(479,120,'CurrentCulture','\"en-US\"'),(480,120,'CurrentUICulture','\"en-US\"'),(481,121,'RecurringJobId','\"DocumentIndexing\"'),(482,121,'Time','1764241230'),(483,121,'CurrentCulture','\"en-US\"'),(484,121,'CurrentUICulture','\"en-US\"'),(485,122,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(486,122,'Time','1764241230'),(487,122,'CurrentCulture','\"en-US\"'),(488,122,'CurrentUICulture','\"en-US\"'),(489,123,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(490,123,'Time','1764241411'),(491,123,'CurrentCulture','\"en-US\"'),(492,123,'CurrentUICulture','\"en-US\"'),(493,124,'RecurringJobId','\"SendEmailReminderForWorkflowTransition\"'),(494,124,'Time','1764241564'),(495,124,'CurrentCulture','\"en-US\"'),(496,124,'CurrentUICulture','\"en-US\"');
/*!40000 ALTER TABLE `HangfireJobParameter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireJobQueue`
--

DROP TABLE IF EXISTS `HangfireJobQueue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireJobQueue` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `FetchedAt` datetime(6) DEFAULT NULL,
  `Queue` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `FetchToken` varchar(36) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_HangfireJobQueue_QueueAndFetchedAt` (`Queue`,`FetchedAt`)
) ENGINE=InnoDB AUTO_INCREMENT=125 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireJobQueue`
--

LOCK TABLES `HangfireJobQueue` WRITE;
/*!40000 ALTER TABLE `HangfireJobQueue` DISABLE KEYS */;
/*!40000 ALTER TABLE `HangfireJobQueue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireJobState`
--

DROP TABLE IF EXISTS `HangfireJobState`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireJobState` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Name` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Reason` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_HangfireJobState_Job` (`JobId`),
  CONSTRAINT `FK_HangfireJobState_Job` FOREIGN KEY (`JobId`) REFERENCES `HangfireJob` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireJobState`
--

LOCK TABLES `HangfireJobState` WRITE;
/*!40000 ALTER TABLE `HangfireJobState` DISABLE KEYS */;
/*!40000 ALTER TABLE `HangfireJobState` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireList`
--

DROP TABLE IF EXISTS `HangfireList`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireList` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireList`
--

LOCK TABLES `HangfireList` WRITE;
/*!40000 ALTER TABLE `HangfireList` DISABLE KEYS */;
/*!40000 ALTER TABLE `HangfireList` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireServer`
--

DROP TABLE IF EXISTS `HangfireServer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireServer` (
  `Id` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Data` longtext NOT NULL,
  `LastHeartbeat` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireServer`
--

LOCK TABLES `HangfireServer` WRITE;
/*!40000 ALTER TABLE `HangfireServer` DISABLE KEYS */;
/*!40000 ALTER TABLE `HangfireServer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireSet`
--

DROP TABLE IF EXISTS `HangfireSet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireSet` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` varchar(256) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Score` float NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_HangfireSet_Key_Value` (`Key`,`Value`)
) ENGINE=InnoDB AUTO_INCREMENT=64136 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireSet`
--

LOCK TABLES `HangfireSet` WRITE;
/*!40000 ALTER TABLE `HangfireSet` DISABLE KEYS */;
INSERT INTO `HangfireSet` VALUES (1,'recurring-jobs','DailyReminder',1764280000,NULL),(2,'recurring-jobs','WeeklyReminder',1764280000,NULL),(3,'recurring-jobs','MonthlyReminder',1764280000,NULL),(4,'recurring-jobs','QuarterlyReminder',1764280000,NULL),(5,'recurring-jobs','HalfYearlyReminder',1764280000,NULL),(6,'recurring-jobs','YearlyReminder',1764280000,NULL),(7,'recurring-jobs','CustomDateReminder',1764280000,NULL),(8,'recurring-jobs','ReminderSchedule',1764240000,NULL),(9,'recurring-jobs','SendEmailScheduler',1764240000,NULL),(10,'recurring-jobs','DocumentIndexing',1764240000,NULL),(11,'recurring-jobs','SendEmailReminderForWorkflowTransition',1764240000,NULL),(12,'recurring-jobs','PermissionCleanup',1764280000,NULL),(13,'recurring-jobs','ArchiveRetentionDocumentToDelete',1764280000,NULL),(14,'recurring-jobs','ArchieveOrDeleteDocummentBaseOnConfigurationOnDocumentOnAction',1764280000,NULL);
/*!40000 ALTER TABLE `HangfireSet` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HangfireState`
--

DROP TABLE IF EXISTS `HangfireState`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HangfireState` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `Name` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Reason` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_HangfireHangFire_State_Job` (`JobId`),
  CONSTRAINT `FK_HangfireHangFire_State_Job` FOREIGN KEY (`JobId`) REFERENCES `HangfireJob` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=373 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HangfireState`
--

LOCK TABLES `HangfireState` WRITE;
/*!40000 ALTER TABLE `HangfireState` DISABLE KEYS */;
INSERT INTO `HangfireState` VALUES (1,1,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:20:21.219017','{\"EnqueuedAt\":\"1764228021189\",\"Queue\":\"reminder\"}'),(2,1,'Processing',NULL,'2025-11-27 07:20:32.031991','{\"StartedAt\":\"1764228031974\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"1b9a2a39-efb8-48ce-b168-b8b9d21ce60d\"}'),(3,1,'Succeeded',NULL,'2025-11-27 07:20:33.713874','{\"SucceededAt\":\"1764228033655\",\"PerformanceDuration\":\"1542\",\"Latency\":\"11048\",\"Result\":\"true\"}'),(4,2,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:21:37.172154','{\"EnqueuedAt\":\"1764228097170\",\"Queue\":\"workflow\"}'),(5,2,'Processing',NULL,'2025-11-27 07:21:47.016097','{\"StartedAt\":\"1764228107002\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"fa0eb52a-7b77-4f2e-9778-f910e25024ad\"}'),(6,2,'Succeeded',NULL,'2025-11-27 07:21:47.416609','{\"SucceededAt\":\"1764228107402\",\"PerformanceDuration\":\"364\",\"Latency\":\"10199\",\"Result\":\"true\"}'),(7,3,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:24:28.957296','{\"EnqueuedAt\":\"1764228268953\",\"Queue\":\"workflow\"}'),(8,3,'Processing',NULL,'2025-11-27 07:24:32.101778','{\"StartedAt\":\"1764228272086\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"46a1a75e-5367-4fb6-aaca-6312d3599532\"}'),(9,3,'Succeeded',NULL,'2025-11-27 07:24:32.222338','{\"SucceededAt\":\"1764228272202\",\"PerformanceDuration\":\"66\",\"Latency\":\"3211\",\"Result\":\"true\"}'),(10,4,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:28:14.677977','{\"EnqueuedAt\":\"1764228494674\",\"Queue\":\"workflow\"}'),(11,4,'Processing',NULL,'2025-11-27 07:28:17.256557','{\"StartedAt\":\"1764228497202\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"46639ff0-bbf2-4ff3-9626-01c741f74aa3\"}'),(12,4,'Succeeded',NULL,'2025-11-27 07:28:17.578810','{\"SucceededAt\":\"1764228497524\",\"PerformanceDuration\":\"205\",\"Latency\":\"2691\",\"Result\":\"true\"}'),(13,5,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:30:15.157151','{\"EnqueuedAt\":\"1764228615151\",\"Queue\":\"reminder\"}'),(14,6,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:30:15.283760','{\"EnqueuedAt\":\"1764228615276\",\"Queue\":\"emailnotification\"}'),(15,7,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:30:15.413202','{\"EnqueuedAt\":\"1764228615405\",\"Queue\":\"documentindex\"}'),(16,8,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:30:15.534512','{\"EnqueuedAt\":\"1764228615531\",\"Queue\":\"workflow\"}'),(17,5,'Processing',NULL,'2025-11-27 07:30:17.332383','{\"StartedAt\":\"1764228617308\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"f2fbce8c-1724-4faa-a887-4b5594d47875\"}'),(18,7,'Processing',NULL,'2025-11-27 07:30:17.410627','{\"StartedAt\":\"1764228617363\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a6f6d8b-0e2b-4ba9-a572-e78a53d77edd\"}'),(19,8,'Processing',NULL,'2025-11-27 07:30:17.685480','{\"StartedAt\":\"1764228617371\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3c7292c8-a7c0-42fb-8967-71dfdeabd467\"}'),(20,6,'Processing',NULL,'2025-11-27 07:30:17.701028','{\"StartedAt\":\"1764228617349\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b1dfdd0e-6f63-4db3-ad65-abb4b4419b7d\"}'),(21,5,'Succeeded',NULL,'2025-11-27 07:30:17.875562','{\"SucceededAt\":\"1764228617849\",\"PerformanceDuration\":\"479\",\"Latency\":\"2269\",\"Result\":\"true\"}'),(22,7,'Succeeded',NULL,'2025-11-27 07:30:17.973692','{\"SucceededAt\":\"1764228617942\",\"PerformanceDuration\":\"484\",\"Latency\":\"2094\",\"Result\":\"false\"}'),(23,8,'Succeeded',NULL,'2025-11-27 07:30:18.477223','{\"SucceededAt\":\"1764228618428\",\"PerformanceDuration\":\"698\",\"Latency\":\"2243\",\"Result\":\"true\"}'),(24,6,'Succeeded',NULL,'2025-11-27 07:30:18.543483','{\"SucceededAt\":\"1764228618501\",\"PerformanceDuration\":\"733\",\"Latency\":\"2538\",\"Result\":\"true\"}'),(25,9,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:46:34.034113','{\"EnqueuedAt\":\"1764229593987\",\"Queue\":\"workflow\"}'),(26,9,'Processing',NULL,'2025-11-27 07:46:34.933905','{\"StartedAt\":\"1764229594395\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"45099974-05e0-4411-861e-64d7293fa5a2\"}'),(27,10,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:46:35.078549','{\"EnqueuedAt\":\"1764229594768\",\"Queue\":\"reminder\"}'),(28,11,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:46:35.906951','{\"EnqueuedAt\":\"1764229595896\",\"Queue\":\"emailnotification\"}'),(29,9,'Succeeded',NULL,'2025-11-27 07:46:35.922905','{\"SucceededAt\":\"1764229595760\",\"PerformanceDuration\":\"513\",\"Latency\":\"1960\",\"Result\":\"true\"}'),(30,10,'Processing',NULL,'2025-11-27 07:46:35.930000','{\"StartedAt\":\"1764229595840\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"f1b13bb8-c5d4-4391-a1b1-60a61a035990\"}'),(31,11,'Processing',NULL,'2025-11-27 07:46:36.260537','{\"StartedAt\":\"1764229596159\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"45099974-05e0-4411-861e-64d7293fa5a2\"}'),(32,12,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:46:36.342179','{\"EnqueuedAt\":\"1764229596333\",\"Queue\":\"documentindex\"}'),(33,11,'Succeeded',NULL,'2025-11-27 07:46:36.598829','{\"SucceededAt\":\"1764229596580\",\"PerformanceDuration\":\"144\",\"Latency\":\"779\",\"Result\":\"true\"}'),(34,12,'Processing',NULL,'2025-11-27 07:46:36.650478','{\"StartedAt\":\"1764229596641\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"45099974-05e0-4411-861e-64d7293fa5a2\"}'),(35,12,'Succeeded',NULL,'2025-11-27 07:46:36.706319','{\"SucceededAt\":\"1764229596692\",\"PerformanceDuration\":\"21\",\"Latency\":\"422\",\"Result\":\"false\"}'),(36,10,'Succeeded',NULL,'2025-11-27 07:46:36.818791','{\"SucceededAt\":\"1764229596799\",\"PerformanceDuration\":\"694\",\"Latency\":\"1729\",\"Result\":\"true\"}'),(37,13,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:48:18.596395','{\"EnqueuedAt\":\"1764229698595\",\"Queue\":\"workflow\"}'),(38,13,'Processing',NULL,'2025-11-27 07:48:18.998295','{\"StartedAt\":\"1764229698989\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"1b9a2a39-efb8-48ce-b168-b8b9d21ce60d\"}'),(39,13,'Succeeded',NULL,'2025-11-27 07:48:19.046567','{\"SucceededAt\":\"1764229699038\",\"PerformanceDuration\":\"21\",\"Latency\":\"447\",\"Result\":\"true\"}'),(40,14,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:50:35.193980','{\"EnqueuedAt\":\"1764229835192\",\"Queue\":\"reminder\"}'),(41,14,'Processing',NULL,'2025-11-27 07:50:36.113017','{\"StartedAt\":\"1764229836099\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"5cf89ab2-6cb2-41cc-9b5b-e03432f7f91f\"}'),(42,14,'Succeeded',NULL,'2025-11-27 07:50:36.263605','{\"SucceededAt\":\"1764229836237\",\"PerformanceDuration\":\"51\",\"Latency\":\"1107\",\"Result\":\"true\"}'),(43,15,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:51:35.478475','{\"EnqueuedAt\":\"1764229895476\",\"Queue\":\"workflow\"}'),(44,15,'Processing',NULL,'2025-11-27 07:51:36.384646','{\"StartedAt\":\"1764229896376\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"5cf89ab2-6cb2-41cc-9b5b-e03432f7f91f\"}'),(45,15,'Succeeded',NULL,'2025-11-27 07:51:36.438888','{\"SucceededAt\":\"1764229896424\",\"PerformanceDuration\":\"20\",\"Latency\":\"954\",\"Result\":\"true\"}'),(46,16,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:54:08.635927','{\"EnqueuedAt\":\"1764230048633\",\"Queue\":\"workflow\"}'),(47,16,'Processing',NULL,'2025-11-27 07:54:18.005762','{\"StartedAt\":\"1764230057997\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"7256ce4c-4218-401b-9ac8-8732ead56003\"}'),(48,16,'Succeeded',NULL,'2025-11-27 07:54:18.064173','{\"SucceededAt\":\"1764230058054\",\"PerformanceDuration\":\"20\",\"Latency\":\"9425\",\"Result\":\"true\"}'),(49,17,'Enqueued','Triggered by recurring job scheduler','2025-11-27 07:57:55.132175','{\"EnqueuedAt\":\"1764230275130\",\"Queue\":\"workflow\"}'),(50,17,'Processing',NULL,'2025-11-27 07:58:04.108701','{\"StartedAt\":\"1764230284101\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a6f6d8b-0e2b-4ba9-a572-e78a53d77edd\"}'),(51,17,'Succeeded',NULL,'2025-11-27 07:58:04.161397','{\"SucceededAt\":\"1764230284145\",\"PerformanceDuration\":\"19\",\"Latency\":\"9019\",\"Result\":\"true\"}'),(52,18,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:00:10.315333','{\"EnqueuedAt\":\"1764230410314\",\"Queue\":\"reminder\"}'),(53,19,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:00:10.370180','{\"EnqueuedAt\":\"1764230410369\",\"Queue\":\"emailnotification\"}'),(54,20,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:00:10.436032','{\"EnqueuedAt\":\"1764230410435\",\"Queue\":\"documentindex\"}'),(55,21,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:00:10.503064','{\"EnqueuedAt\":\"1764230410502\",\"Queue\":\"workflow\"}'),(56,18,'Processing',NULL,'2025-11-27 08:00:19.142160','{\"StartedAt\":\"1764230419133\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"62bfcebb-6f34-41f5-80f8-41a653cd5b7f\"}'),(57,18,'Succeeded',NULL,'2025-11-27 08:00:19.194189','{\"SucceededAt\":\"1764230419179\",\"PerformanceDuration\":\"18\",\"Latency\":\"8870\",\"Result\":\"true\"}'),(58,21,'Processing',NULL,'2025-11-27 08:00:19.247285','{\"StartedAt\":\"1764230419238\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"62bfcebb-6f34-41f5-80f8-41a653cd5b7f\"}'),(59,21,'Succeeded',NULL,'2025-11-27 08:00:19.307319','{\"SucceededAt\":\"1764230419292\",\"PerformanceDuration\":\"22\",\"Latency\":\"8792\",\"Result\":\"true\"}'),(60,20,'Processing',NULL,'2025-11-27 08:00:19.324439','{\"StartedAt\":\"1764230419196\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"7256ce4c-4218-401b-9ac8-8732ead56003\"}'),(61,20,'Succeeded',NULL,'2025-11-27 08:00:19.390687','{\"SucceededAt\":\"1764230419375\",\"PerformanceDuration\":\"28\",\"Latency\":\"8949\",\"Result\":\"false\"}'),(62,19,'Processing',NULL,'2025-11-27 08:00:19.444676','{\"StartedAt\":\"1764230419188\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"33424b83-4657-4924-9f05-d21144d0e3dd\"}'),(63,19,'Succeeded',NULL,'2025-11-27 08:00:19.537002','{\"SucceededAt\":\"1764230419526\",\"PerformanceDuration\":\"57\",\"Latency\":\"9124\",\"Result\":\"true\"}'),(64,22,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:03:03.481824','{\"EnqueuedAt\":\"1764230583480\",\"Queue\":\"workflow\"}'),(65,22,'Processing',NULL,'2025-11-27 08:03:04.376351','{\"StartedAt\":\"1764230584368\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"62bfcebb-6f34-41f5-80f8-41a653cd5b7f\"}'),(66,22,'Succeeded',NULL,'2025-11-27 08:03:04.422682','{\"SucceededAt\":\"1764230584410\",\"PerformanceDuration\":\"17\",\"Latency\":\"936\",\"Result\":\"true\"}'),(67,23,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:06:33.770621','{\"EnqueuedAt\":\"1764230793769\",\"Queue\":\"workflow\"}'),(68,23,'Processing',NULL,'2025-11-27 08:06:34.712115','{\"StartedAt\":\"1764230794488\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"62bfcebb-6f34-41f5-80f8-41a653cd5b7f\"}'),(69,23,'Succeeded',NULL,'2025-11-27 08:06:34.761127','{\"SucceededAt\":\"1764230794749\",\"PerformanceDuration\":\"19\",\"Latency\":\"985\",\"Result\":\"true\"}'),(70,24,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:09:18.775422','{\"EnqueuedAt\":\"1764230958774\",\"Queue\":\"workflow\"}'),(71,24,'Processing',NULL,'2025-11-27 08:09:19.590801','{\"StartedAt\":\"1764230959571\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b4af9158-7b6c-4a2e-9c5b-883a1968cbd8\"}'),(72,24,'Succeeded',NULL,'2025-11-27 08:09:19.760924','{\"SucceededAt\":\"1764230959730\",\"PerformanceDuration\":\"49\",\"Latency\":\"999\",\"Result\":\"true\"}'),(73,25,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:10:49.068433','{\"EnqueuedAt\":\"1764231049067\",\"Queue\":\"reminder\"}'),(74,25,'Processing',NULL,'2025-11-27 08:10:49.602333','{\"StartedAt\":\"1764231049593\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"7256ce4c-4218-401b-9ac8-8732ead56003\"}'),(75,25,'Succeeded',NULL,'2025-11-27 08:10:49.648899','{\"SucceededAt\":\"1764231049638\",\"PerformanceDuration\":\"16\",\"Latency\":\"579\",\"Result\":\"true\"}'),(76,26,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:12:49.244029','{\"EnqueuedAt\":\"1764231169242\",\"Queue\":\"workflow\"}'),(77,26,'Processing',NULL,'2025-11-27 08:12:49.642989','{\"StartedAt\":\"1764231169636\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"46a1a75e-5367-4fb6-aaca-6312d3599532\"}'),(78,26,'Succeeded',NULL,'2025-11-27 08:12:49.692320','{\"SucceededAt\":\"1764231169680\",\"PerformanceDuration\":\"20\",\"Latency\":\"440\",\"Result\":\"true\"}'),(79,27,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:15:04.435506','{\"EnqueuedAt\":\"1764231304434\",\"Queue\":\"emailnotification\"}'),(80,28,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:15:04.489863','{\"EnqueuedAt\":\"1764231304488\",\"Queue\":\"documentindex\"}'),(81,29,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:15:04.560008','{\"EnqueuedAt\":\"1764231304558\",\"Queue\":\"workflow\"}'),(82,27,'Processing',NULL,'2025-11-27 08:15:04.693725','{\"StartedAt\":\"1764231304686\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"fa0eb52a-7b77-4f2e-9778-f910e25024ad\"}'),(83,28,'Processing',NULL,'2025-11-27 08:15:04.826018','{\"StartedAt\":\"1764231304694\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b1dfdd0e-6f63-4db3-ad65-abb4b4419b7d\"}'),(84,28,'Succeeded',NULL,'2025-11-27 08:15:04.873417','{\"SucceededAt\":\"1764231304862\",\"PerformanceDuration\":\"17\",\"Latency\":\"378\",\"Result\":\"false\"}'),(85,29,'Processing',NULL,'2025-11-27 08:15:04.926400','{\"StartedAt\":\"1764231304700\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"920bd413-0e2b-4066-a64a-723b5a9ba9dc\"}'),(86,29,'Succeeded',NULL,'2025-11-27 08:15:04.967659','{\"SucceededAt\":\"1764231304958\",\"PerformanceDuration\":\"15\",\"Latency\":\"408\",\"Result\":\"true\"}'),(87,27,'Succeeded',NULL,'2025-11-27 08:15:05.307567','{\"SucceededAt\":\"1764231304745\",\"PerformanceDuration\":\"21\",\"Latency\":\"312\",\"Result\":\"true\"}'),(88,30,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:18:00.906566','{\"EnqueuedAt\":\"1764231480905\",\"Queue\":\"workflow\"}'),(89,30,'Processing',NULL,'2025-11-27 08:18:04.895796','{\"StartedAt\":\"1764231484885\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"39ed4cee-91c6-4856-9898-a9fa623cb5ad\"}'),(90,30,'Succeeded',NULL,'2025-11-27 08:18:04.938392','{\"SucceededAt\":\"1764231484930\",\"PerformanceDuration\":\"16\",\"Latency\":\"4030\",\"Result\":\"true\"}'),(91,31,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:20:04.950779','{\"EnqueuedAt\":\"1764231604949\",\"Queue\":\"reminder\"}'),(92,31,'Processing',NULL,'2025-11-27 08:20:05.056935','{\"StartedAt\":\"1764231605026\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b1dfdd0e-6f63-4db3-ad65-abb4b4419b7d\"}'),(93,31,'Succeeded',NULL,'2025-11-27 08:20:05.221391','{\"SucceededAt\":\"1764231605194\",\"PerformanceDuration\":\"60\",\"Latency\":\"299\",\"Result\":\"true\"}'),(94,32,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:21:20.247110','{\"EnqueuedAt\":\"1764231680246\",\"Queue\":\"workflow\"}'),(95,32,'Processing',NULL,'2025-11-27 08:21:20.329656','{\"StartedAt\":\"1764231680321\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"46a1a75e-5367-4fb6-aaca-6312d3599532\"}'),(96,32,'Succeeded',NULL,'2025-11-27 08:21:20.389463','{\"SucceededAt\":\"1764231680376\",\"PerformanceDuration\":\"29\",\"Latency\":\"132\",\"Result\":\"true\"}'),(97,33,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:24:09.053069','{\"EnqueuedAt\":\"1764231849052\",\"Queue\":\"workflow\"}'),(98,33,'Processing',NULL,'2025-11-27 08:24:20.111945','{\"StartedAt\":\"1764231860101\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3eb92668-8a58-4a4d-909c-cbc736f98618\"}'),(99,33,'Succeeded',NULL,'2025-11-27 08:24:20.167880','{\"SucceededAt\":\"1764231860156\",\"PerformanceDuration\":\"25\",\"Latency\":\"11106\",\"Result\":\"true\"}'),(100,34,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:27:54.327913','{\"EnqueuedAt\":\"1764232074326\",\"Queue\":\"workflow\"}'),(101,34,'Processing',NULL,'2025-11-27 08:28:05.145988','{\"StartedAt\":\"1764232085138\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"62bfcebb-6f34-41f5-80f8-41a653cd5b7f\"}'),(102,34,'Succeeded',NULL,'2025-11-27 08:28:05.198859','{\"SucceededAt\":\"1764232085188\",\"PerformanceDuration\":\"25\",\"Latency\":\"10861\",\"Result\":\"true\"}'),(103,35,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:30:11.347408','{\"EnqueuedAt\":\"1764232211346\",\"Queue\":\"reminder\"}'),(104,36,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:30:11.397449','{\"EnqueuedAt\":\"1764232211396\",\"Queue\":\"emailnotification\"}'),(105,37,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:30:11.448998','{\"EnqueuedAt\":\"1764232211448\",\"Queue\":\"documentindex\"}'),(106,38,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:30:11.499025','{\"EnqueuedAt\":\"1764232211498\",\"Queue\":\"workflow\"}'),(107,35,'Processing',NULL,'2025-11-27 08:30:20.172891','{\"StartedAt\":\"1764232220166\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b4af9158-7b6c-4a2e-9c5b-883a1968cbd8\"}'),(108,36,'Processing',NULL,'2025-11-27 08:30:20.204703','{\"StartedAt\":\"1764232220193\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"920bd413-0e2b-4066-a64a-723b5a9ba9dc\"}'),(109,35,'Succeeded',NULL,'2025-11-27 08:30:20.226931','{\"SucceededAt\":\"1764232220214\",\"PerformanceDuration\":\"23\",\"Latency\":\"8868\",\"Result\":\"true\"}'),(110,36,'Succeeded',NULL,'2025-11-27 08:30:20.267751','{\"SucceededAt\":\"1764232220249\",\"PerformanceDuration\":\"19\",\"Latency\":\"8854\",\"Result\":\"true\"}'),(111,37,'Processing',NULL,'2025-11-27 08:30:20.271181','{\"StartedAt\":\"1764232220259\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3eb92668-8a58-4a4d-909c-cbc736f98618\"}'),(112,38,'Processing',NULL,'2025-11-27 08:30:20.297979','{\"StartedAt\":\"1764232220266\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"7256ce4c-4218-401b-9ac8-8732ead56003\"}'),(113,37,'Succeeded',NULL,'2025-11-27 08:30:20.334564','{\"SucceededAt\":\"1764232220324\",\"PerformanceDuration\":\"22\",\"Latency\":\"8877\",\"Result\":\"false\"}'),(114,38,'Succeeded',NULL,'2025-11-27 08:30:20.364199','{\"SucceededAt\":\"1764232220351\",\"PerformanceDuration\":\"34\",\"Latency\":\"8840\",\"Result\":\"true\"}'),(115,39,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:33:44.006214','{\"EnqueuedAt\":\"1764232424005\",\"Queue\":\"workflow\"}'),(116,39,'Processing',NULL,'2025-11-27 08:33:50.367819','{\"StartedAt\":\"1764232430341\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b4af9158-7b6c-4a2e-9c5b-883a1968cbd8\"}'),(117,39,'Succeeded',NULL,'2025-11-27 08:33:50.527363','{\"SucceededAt\":\"1764232430498\",\"PerformanceDuration\":\"60\",\"Latency\":\"6525\",\"Result\":\"true\"}'),(118,40,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:36:29.394583','{\"EnqueuedAt\":\"1764232589392\",\"Queue\":\"workflow\"}'),(119,40,'Processing',NULL,'2025-11-27 08:36:35.414555','{\"StartedAt\":\"1764232595404\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a0e0b2b-1d84-498f-8708-cbf512016eac\"}'),(120,40,'Succeeded',NULL,'2025-11-27 08:36:35.460305','{\"SucceededAt\":\"1764232595450\",\"PerformanceDuration\":\"19\",\"Latency\":\"6063\",\"Result\":\"true\"}'),(121,41,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:39:05.262575','{\"EnqueuedAt\":\"1764232745261\",\"Queue\":\"workflow\"}'),(122,41,'Processing',NULL,'2025-11-27 08:39:05.444363','{\"StartedAt\":\"1764232745433\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"920bd413-0e2b-4066-a64a-723b5a9ba9dc\"}'),(123,41,'Succeeded',NULL,'2025-11-27 08:39:05.489860','{\"SucceededAt\":\"1764232745481\",\"PerformanceDuration\":\"17\",\"Latency\":\"226\",\"Result\":\"true\"}'),(124,42,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:40:35.413983','{\"EnqueuedAt\":\"1764232835413\",\"Queue\":\"reminder\"}'),(125,42,'Processing',NULL,'2025-11-27 08:40:35.462748','{\"StartedAt\":\"1764232835456\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"62bfcebb-6f34-41f5-80f8-41a653cd5b7f\"}'),(126,42,'Succeeded',NULL,'2025-11-27 08:40:35.507983','{\"SucceededAt\":\"1764232835497\",\"PerformanceDuration\":\"16\",\"Latency\":\"90\",\"Result\":\"true\"}'),(127,43,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:42:50.620191','{\"EnqueuedAt\":\"1764232970618\",\"Queue\":\"workflow\"}'),(128,43,'Processing',NULL,'2025-11-27 08:42:50.670309','{\"StartedAt\":\"1764232970656\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"39ed4cee-91c6-4856-9898-a9fa623cb5ad\"}'),(129,43,'Succeeded',NULL,'2025-11-27 08:42:50.719350','{\"SucceededAt\":\"1764232970709\",\"PerformanceDuration\":\"18\",\"Latency\":\"97\",\"Result\":\"true\"}'),(130,44,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:45:06.928110','{\"EnqueuedAt\":\"1764233106927\",\"Queue\":\"emailnotification\"}'),(131,45,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:45:06.980343','{\"EnqueuedAt\":\"1764233106979\",\"Queue\":\"documentindex\"}'),(132,46,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:45:07.030099','{\"EnqueuedAt\":\"1764233107029\",\"Queue\":\"workflow\"}'),(133,44,'Processing',NULL,'2025-11-27 08:45:08.215506','{\"StartedAt\":\"1764233108209\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"5cf89ab2-6cb2-41cc-9b5b-e03432f7f91f\"}'),(134,44,'Succeeded',NULL,'2025-11-27 08:45:08.253840','{\"SucceededAt\":\"1764233108245\",\"PerformanceDuration\":\"14\",\"Latency\":\"1329\",\"Result\":\"true\"}'),(135,45,'Processing',NULL,'2025-11-27 08:45:08.290566','{\"StartedAt\":\"1764233108283\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"5cf89ab2-6cb2-41cc-9b5b-e03432f7f91f\"}'),(136,45,'Succeeded',NULL,'2025-11-27 08:45:08.328063','{\"SucceededAt\":\"1764233108319\",\"PerformanceDuration\":\"13\",\"Latency\":\"1350\",\"Result\":\"false\"}'),(137,46,'Processing',NULL,'2025-11-27 08:45:08.364804','{\"StartedAt\":\"1764233108356\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"5cf89ab2-6cb2-41cc-9b5b-e03432f7f91f\"}'),(138,46,'Succeeded',NULL,'2025-11-27 08:45:08.425408','{\"SucceededAt\":\"1764233108417\",\"PerformanceDuration\":\"14\",\"Latency\":\"1394\",\"Result\":\"true\"}'),(139,47,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:48:15.005045','{\"EnqueuedAt\":\"1764233295003\",\"Queue\":\"workflow\"}'),(140,47,'Processing',NULL,'2025-11-27 08:48:20.630592','{\"StartedAt\":\"1764233300620\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3eb92668-8a58-4a4d-909c-cbc736f98618\"}'),(141,47,'Succeeded',NULL,'2025-11-27 08:48:20.721305','{\"SucceededAt\":\"1764233300700\",\"PerformanceDuration\":\"45\",\"Latency\":\"5681\",\"Result\":\"true\"}'),(142,48,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:50:00.829767','{\"EnqueuedAt\":\"1764233400826\",\"Queue\":\"reminder\"}'),(143,48,'Processing',NULL,'2025-11-27 08:50:05.674776','{\"StartedAt\":\"1764233405663\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"920bd413-0e2b-4066-a64a-723b5a9ba9dc\"}'),(144,48,'Succeeded',NULL,'2025-11-27 08:50:05.752774','{\"SucceededAt\":\"1764233405731\",\"PerformanceDuration\":\"32\",\"Latency\":\"4902\",\"Result\":\"true\"}'),(145,49,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:51:16.058825','{\"EnqueuedAt\":\"1764233476057\",\"Queue\":\"workflow\"}'),(146,49,'Processing',NULL,'2025-11-27 08:51:20.732831','{\"StartedAt\":\"1764233480715\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a0e0b2b-1d84-498f-8708-cbf512016eac\"}'),(147,49,'Succeeded',NULL,'2025-11-27 08:51:20.806111','{\"SucceededAt\":\"1764233480787\",\"PerformanceDuration\":\"32\",\"Latency\":\"4737\",\"Result\":\"true\"}'),(148,50,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:54:24.056021','{\"EnqueuedAt\":\"1764233664054\",\"Queue\":\"workflow\"}'),(149,50,'Processing',NULL,'2025-11-27 08:54:35.871110','{\"StartedAt\":\"1764233675851\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"62bfcebb-6f34-41f5-80f8-41a653cd5b7f\"}'),(150,50,'Succeeded',NULL,'2025-11-27 08:54:36.031677','{\"SucceededAt\":\"1764233676013\",\"PerformanceDuration\":\"68\",\"Latency\":\"11957\",\"Result\":\"true\"}'),(151,51,'Enqueued','Triggered by recurring job scheduler','2025-11-27 08:57:39.952709','{\"EnqueuedAt\":\"1764233859950\",\"Queue\":\"workflow\"}'),(152,51,'Processing',NULL,'2025-11-27 08:57:50.942453','{\"StartedAt\":\"1764233870927\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b1dfdd0e-6f63-4db3-ad65-abb4b4419b7d\"}'),(153,51,'Succeeded',NULL,'2025-11-27 08:57:51.102335','{\"SucceededAt\":\"1764233871075\",\"PerformanceDuration\":\"64\",\"Latency\":\"11161\",\"Result\":\"true\"}'),(154,52,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:00:18.015860','{\"EnqueuedAt\":\"1764234018014\",\"Queue\":\"reminder\"}'),(155,53,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:00:18.078305','{\"EnqueuedAt\":\"1764234018077\",\"Queue\":\"emailnotification\"}'),(156,54,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:00:18.148926','{\"EnqueuedAt\":\"1764234018146\",\"Queue\":\"documentindex\"}'),(157,55,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:00:18.210750','{\"EnqueuedAt\":\"1764234018209\",\"Queue\":\"workflow\"}'),(158,52,'Processing',NULL,'2025-11-27 09:00:21.016946','{\"StartedAt\":\"1764234021009\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a0e0b2b-1d84-498f-8708-cbf512016eac\"}'),(159,52,'Succeeded',NULL,'2025-11-27 09:00:21.063400','{\"SucceededAt\":\"1764234021051\",\"PerformanceDuration\":\"15\",\"Latency\":\"3050\",\"Result\":\"true\"}'),(160,53,'Processing',NULL,'2025-11-27 09:00:21.429400','{\"StartedAt\":\"1764234021065\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"920bd413-0e2b-4066-a64a-723b5a9ba9dc\"}'),(161,53,'Succeeded',NULL,'2025-11-27 09:00:21.490082','{\"SucceededAt\":\"1764234021469\",\"PerformanceDuration\":\"18\",\"Latency\":\"3399\",\"Result\":\"true\"}'),(162,54,'Processing',NULL,'2025-11-27 09:00:21.603235','{\"StartedAt\":\"1764234021074\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"7256ce4c-4218-401b-9ac8-8732ead56003\"}'),(163,54,'Succeeded',NULL,'2025-11-27 09:00:21.660017','{\"SucceededAt\":\"1764234021641\",\"PerformanceDuration\":\"17\",\"Latency\":\"3507\",\"Result\":\"false\"}'),(164,55,'Processing',NULL,'2025-11-27 09:00:22.534390','{\"StartedAt\":\"1764234021111\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a0e0b2b-1d84-498f-8708-cbf512016eac\"}'),(165,55,'Succeeded',NULL,'2025-11-27 09:00:22.604654','{\"SucceededAt\":\"1764234022590\",\"PerformanceDuration\":\"33\",\"Latency\":\"4373\",\"Result\":\"true\"}'),(166,56,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:04:03.568056','{\"EnqueuedAt\":\"1764234243566\",\"Queue\":\"workflow\"}'),(167,56,'Processing',NULL,'2025-11-27 09:04:06.361419','{\"StartedAt\":\"1764234246346\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3eb92668-8a58-4a4d-909c-cbc736f98618\"}'),(168,56,'Succeeded',NULL,'2025-11-27 09:04:06.485207','{\"SucceededAt\":\"1764234246465\",\"PerformanceDuration\":\"71\",\"Latency\":\"2854\",\"Result\":\"true\"}'),(169,57,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:06:18.827668','{\"EnqueuedAt\":\"1764234378826\",\"Queue\":\"workflow\"}'),(170,57,'Processing',NULL,'2025-11-27 09:06:21.475144','{\"StartedAt\":\"1764234381464\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"46a1a75e-5367-4fb6-aaca-6312d3599532\"}'),(171,57,'Succeeded',NULL,'2025-11-27 09:06:21.529413','{\"SucceededAt\":\"1764234381516\",\"PerformanceDuration\":\"20\",\"Latency\":\"2701\",\"Result\":\"true\"}'),(172,58,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:09:08.431152','{\"EnqueuedAt\":\"1764234548428\",\"Queue\":\"workflow\"}'),(173,58,'Processing',NULL,'2025-11-27 09:09:08.936581','{\"StartedAt\":\"1764234548927\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"45099974-05e0-4411-861e-64d7293fa5a2\"}'),(174,58,'Succeeded',NULL,'2025-11-27 09:09:08.992687','{\"SucceededAt\":\"1764234548982\",\"PerformanceDuration\":\"24\",\"Latency\":\"554\",\"Result\":\"true\"}'),(175,59,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:10:38.641637','{\"EnqueuedAt\":\"1764234638636\",\"Queue\":\"reminder\"}'),(176,59,'Processing',NULL,'2025-11-27 09:10:39.033299','{\"StartedAt\":\"1764234639024\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"5cf89ab2-6cb2-41cc-9b5b-e03432f7f91f\"}'),(177,59,'Succeeded',NULL,'2025-11-27 09:10:39.099750','{\"SucceededAt\":\"1764234639084\",\"PerformanceDuration\":\"27\",\"Latency\":\"456\",\"Result\":\"true\"}'),(178,60,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:12:38.908320','{\"EnqueuedAt\":\"1764234758906\",\"Queue\":\"workflow\"}'),(179,60,'Processing',NULL,'2025-11-27 09:12:39.099738','{\"StartedAt\":\"1764234759085\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"45099974-05e0-4411-861e-64d7293fa5a2\"}'),(180,60,'Succeeded',NULL,'2025-11-27 09:12:39.164590','{\"SucceededAt\":\"1764234759145\",\"PerformanceDuration\":\"25\",\"Latency\":\"244\",\"Result\":\"true\"}'),(181,61,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:15:22.194279','{\"EnqueuedAt\":\"1764234922190\",\"Queue\":\"emailnotification\"}'),(182,61,'Processing',NULL,'2025-11-27 09:15:22.240103','{\"StartedAt\":\"1764234922230\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"f2fbce8c-1724-4faa-a887-4b5594d47875\"}'),(183,62,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:15:22.620555','{\"EnqueuedAt\":\"1764234922618\",\"Queue\":\"documentindex\"}'),(184,63,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:15:22.722176','{\"EnqueuedAt\":\"1764234922719\",\"Queue\":\"workflow\"}'),(185,62,'Processing',NULL,'2025-11-27 09:15:22.773214','{\"StartedAt\":\"1764234922729\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"1b9a2a39-efb8-48ce-b168-b8b9d21ce60d\"}'),(186,63,'Processing',NULL,'2025-11-27 09:15:22.844147','{\"StartedAt\":\"1764234922827\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"920bd413-0e2b-4066-a64a-723b5a9ba9dc\"}'),(187,62,'Succeeded',NULL,'2025-11-27 09:15:22.849485','{\"SucceededAt\":\"1764234922827\",\"PerformanceDuration\":\"27\",\"Latency\":\"207\",\"Result\":\"false\"}'),(188,63,'Succeeded',NULL,'2025-11-27 09:15:22.924731','{\"SucceededAt\":\"1764234922910\",\"PerformanceDuration\":\"37\",\"Latency\":\"204\",\"Result\":\"true\"}'),(189,61,'Succeeded',NULL,'2025-11-27 09:15:23.087524','{\"SucceededAt\":\"1764234922289\",\"PerformanceDuration\":\"22\",\"Latency\":\"107\",\"Result\":\"true\"}'),(190,64,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:19:08.087173','{\"EnqueuedAt\":\"1764235148086\",\"Queue\":\"workflow\"}'),(191,64,'Processing',NULL,'2025-11-27 09:19:08.179692','{\"StartedAt\":\"1764235148170\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"62bfcebb-6f34-41f5-80f8-41a653cd5b7f\"}'),(192,64,'Succeeded',NULL,'2025-11-27 09:19:08.232231','{\"SucceededAt\":\"1764235148222\",\"PerformanceDuration\":\"21\",\"Latency\":\"141\",\"Result\":\"true\"}'),(193,65,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:21:08.293010','{\"EnqueuedAt\":\"1764235268292\",\"Queue\":\"reminder\"}'),(194,66,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:21:08.350772','{\"EnqueuedAt\":\"1764235268349\",\"Queue\":\"workflow\"}'),(195,65,'Processing',NULL,'2025-11-27 09:21:08.534539','{\"StartedAt\":\"1764235268353\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b1dfdd0e-6f63-4db3-ad65-abb4b4419b7d\"}'),(196,65,'Succeeded',NULL,'2025-11-27 09:21:08.605117','{\"SucceededAt\":\"1764235268588\",\"PerformanceDuration\":\"34\",\"Latency\":\"287\",\"Result\":\"true\"}'),(197,66,'Processing',NULL,'2025-11-27 09:21:08.831802','{\"StartedAt\":\"1764235268499\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"33424b83-4657-4924-9f05-d21144d0e3dd\"}'),(198,66,'Succeeded',NULL,'2025-11-27 09:21:08.888201','{\"SucceededAt\":\"1764235268874\",\"PerformanceDuration\":\"23\",\"Latency\":\"526\",\"Result\":\"true\"}'),(199,67,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:24:05.488686','{\"EnqueuedAt\":\"1764235445487\",\"Queue\":\"workflow\"}'),(200,67,'Processing',NULL,'2025-11-27 09:24:06.828152','{\"StartedAt\":\"1764235446820\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a6f6d8b-0e2b-4ba9-a572-e78a53d77edd\"}'),(201,67,'Succeeded',NULL,'2025-11-27 09:24:06.882576','{\"SucceededAt\":\"1764235446871\",\"PerformanceDuration\":\"24\",\"Latency\":\"1385\",\"Result\":\"true\"}'),(202,68,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:27:35.972127','{\"EnqueuedAt\":\"1764235655971\",\"Queue\":\"workflow\"}'),(203,68,'Processing',NULL,'2025-11-27 09:27:36.904141','{\"StartedAt\":\"1764235656895\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3c7292c8-a7c0-42fb-8967-71dfdeabd467\"}'),(204,68,'Succeeded',NULL,'2025-11-27 09:27:36.959864','{\"SucceededAt\":\"1764235656946\",\"PerformanceDuration\":\"21\",\"Latency\":\"986\",\"Result\":\"true\"}'),(205,69,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:30:17.613583','{\"EnqueuedAt\":\"1764235817612\",\"Queue\":\"reminder\"}'),(206,70,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:30:17.675465','{\"EnqueuedAt\":\"1764235817674\",\"Queue\":\"emailnotification\"}'),(207,71,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:30:17.732220','{\"EnqueuedAt\":\"1764235817731\",\"Queue\":\"documentindex\"}'),(208,72,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:30:17.793741','{\"EnqueuedAt\":\"1764235817792\",\"Queue\":\"workflow\"}'),(209,69,'Processing',NULL,'2025-11-27 09:30:21.982996','{\"StartedAt\":\"1764235821971\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3eb92668-8a58-4a4d-909c-cbc736f98618\"}'),(210,70,'Processing',NULL,'2025-11-27 09:30:22.007596','{\"StartedAt\":\"1764235821995\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a6f6d8b-0e2b-4ba9-a572-e78a53d77edd\"}'),(211,69,'Succeeded',NULL,'2025-11-27 09:30:22.073305','{\"SucceededAt\":\"1764235822038\",\"PerformanceDuration\":\"24\",\"Latency\":\"4428\",\"Result\":\"true\"}'),(212,71,'Processing',NULL,'2025-11-27 09:30:22.086454','{\"StartedAt\":\"1764235822068\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3c7292c8-a7c0-42fb-8967-71dfdeabd467\"}'),(213,70,'Succeeded',NULL,'2025-11-27 09:30:22.104890','{\"SucceededAt\":\"1764235822079\",\"PerformanceDuration\":\"46\",\"Latency\":\"4388\",\"Result\":\"true\"}'),(214,72,'Processing',NULL,'2025-11-27 09:30:22.155431','{\"StartedAt\":\"1764235822143\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3eb92668-8a58-4a4d-909c-cbc736f98618\"}'),(215,71,'Succeeded',NULL,'2025-11-27 09:30:22.172599','{\"SucceededAt\":\"1764235822160\",\"PerformanceDuration\":\"28\",\"Latency\":\"4424\",\"Result\":\"false\"}'),(216,72,'Succeeded',NULL,'2025-11-27 09:30:22.223225','{\"SucceededAt\":\"1764235822210\",\"PerformanceDuration\":\"30\",\"Latency\":\"4415\",\"Result\":\"true\"}'),(217,73,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:34:03.387260','{\"EnqueuedAt\":\"1764236043386\",\"Queue\":\"workflow\"}'),(218,73,'Processing',NULL,'2025-11-27 09:34:07.230862','{\"StartedAt\":\"1764236047222\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a6f6d8b-0e2b-4ba9-a572-e78a53d77edd\"}'),(219,73,'Succeeded',NULL,'2025-11-27 09:34:07.290771','{\"SucceededAt\":\"1764236047278\",\"PerformanceDuration\":\"29\",\"Latency\":\"3887\",\"Result\":\"true\"}'),(220,74,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:36:03.614981','{\"EnqueuedAt\":\"1764236163613\",\"Queue\":\"workflow\"}'),(221,74,'Processing',NULL,'2025-11-27 09:36:07.323176','{\"StartedAt\":\"1764236167310\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b4af9158-7b6c-4a2e-9c5b-883a1968cbd8\"}'),(222,74,'Succeeded',NULL,'2025-11-27 09:36:07.403744','{\"SucceededAt\":\"1764236167381\",\"PerformanceDuration\":\"33\",\"Latency\":\"3763\",\"Result\":\"true\"}'),(223,75,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:39:16.612036','{\"EnqueuedAt\":\"1764236356610\",\"Queue\":\"workflow\"}'),(224,75,'Processing',NULL,'2025-11-27 09:39:22.388569','{\"StartedAt\":\"1764236362373\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3c7292c8-a7c0-42fb-8967-71dfdeabd467\"}'),(225,75,'Succeeded',NULL,'2025-11-27 09:39:22.466823','{\"SucceededAt\":\"1764236362444\",\"PerformanceDuration\":\"30\",\"Latency\":\"5826\",\"Result\":\"true\"}'),(226,76,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:40:16.760392','{\"EnqueuedAt\":\"1764236416758\",\"Queue\":\"reminder\"}'),(227,76,'Processing',NULL,'2025-11-27 09:40:22.424697','{\"StartedAt\":\"1764236422414\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3eb92668-8a58-4a4d-909c-cbc736f98618\"}'),(228,76,'Succeeded',NULL,'2025-11-27 09:40:22.484098','{\"SucceededAt\":\"1764236422469\",\"PerformanceDuration\":\"23\",\"Latency\":\"5721\",\"Result\":\"true\"}'),(229,77,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:42:31.982501','{\"EnqueuedAt\":\"1764236551981\",\"Queue\":\"workflow\"}'),(230,77,'Processing',NULL,'2025-11-27 09:42:37.486559','{\"StartedAt\":\"1764236557470\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a6f6d8b-0e2b-4ba9-a572-e78a53d77edd\"}'),(231,77,'Succeeded',NULL,'2025-11-27 09:42:37.773320','{\"SucceededAt\":\"1764236557757\",\"PerformanceDuration\":\"242\",\"Latency\":\"5556\",\"Result\":\"true\"}'),(232,78,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:45:10.384385','{\"EnqueuedAt\":\"1764236710383\",\"Queue\":\"emailnotification\"}'),(233,79,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:45:10.440162','{\"EnqueuedAt\":\"1764236710439\",\"Queue\":\"documentindex\"}'),(234,80,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:45:10.499695','{\"EnqueuedAt\":\"1764236710498\",\"Queue\":\"workflow\"}'),(235,80,'Processing',NULL,'2025-11-27 09:45:22.674593','{\"StartedAt\":\"1764236722629\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b4af9158-7b6c-4a2e-9c5b-883a1968cbd8\"}'),(236,78,'Processing',NULL,'2025-11-27 09:45:22.681627','{\"StartedAt\":\"1764236722604\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3eb92668-8a58-4a4d-909c-cbc736f98618\"}'),(237,79,'Processing',NULL,'2025-11-27 09:45:22.703510','{\"StartedAt\":\"1764236722617\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"46a1a75e-5367-4fb6-aaca-6312d3599532\"}'),(238,78,'Succeeded',NULL,'2025-11-27 09:45:22.776149','{\"SucceededAt\":\"1764236722758\",\"PerformanceDuration\":\"36\",\"Latency\":\"12362\",\"Result\":\"true\"}'),(239,80,'Succeeded',NULL,'2025-11-27 09:45:22.794086','{\"SucceededAt\":\"1764236722776\",\"PerformanceDuration\":\"67\",\"Latency\":\"12235\",\"Result\":\"true\"}'),(240,79,'Succeeded',NULL,'2025-11-27 09:45:22.809016','{\"SucceededAt\":\"1764236722791\",\"PerformanceDuration\":\"43\",\"Latency\":\"12334\",\"Result\":\"false\"}'),(241,81,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:48:55.857967','{\"EnqueuedAt\":\"1764236935857\",\"Queue\":\"workflow\"}'),(242,81,'Processing',NULL,'2025-11-27 09:49:08.056060','{\"StartedAt\":\"1764236947886\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3eb92668-8a58-4a4d-909c-cbc736f98618\"}'),(243,81,'Succeeded',NULL,'2025-11-27 09:49:08.101652','{\"SucceededAt\":\"1764236948091\",\"PerformanceDuration\":\"19\",\"Latency\":\"12239\",\"Result\":\"true\"}'),(244,82,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:50:56.088500','{\"EnqueuedAt\":\"1764237056086\",\"Queue\":\"reminder\"}'),(245,82,'Processing',NULL,'2025-11-27 09:51:07.934892','{\"StartedAt\":\"1764237067925\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b4af9158-7b6c-4a2e-9c5b-883a1968cbd8\"}'),(246,82,'Succeeded',NULL,'2025-11-27 09:51:07.988373','{\"SucceededAt\":\"1764237067977\",\"PerformanceDuration\":\"21\",\"Latency\":\"11911\",\"Result\":\"true\"}'),(247,83,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:51:15.597185','{\"EnqueuedAt\":\"1764237075596\",\"Queue\":\"workflow\"}'),(248,83,'Processing',NULL,'2025-11-27 09:51:22.978973','{\"StartedAt\":\"1764237082972\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a6f6d8b-0e2b-4ba9-a572-e78a53d77edd\"}'),(249,83,'Succeeded',NULL,'2025-11-27 09:51:23.025351','{\"SucceededAt\":\"1764237083016\",\"PerformanceDuration\":\"18\",\"Latency\":\"7427\",\"Result\":\"true\"}'),(250,84,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:54:04.127718','{\"EnqueuedAt\":\"1764237244126\",\"Queue\":\"workflow\"}'),(251,84,'Processing',NULL,'2025-11-27 09:54:08.074376','{\"StartedAt\":\"1764237248042\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"46a1a75e-5367-4fb6-aaca-6312d3599532\"}'),(252,84,'Succeeded',NULL,'2025-11-27 09:54:08.332683','{\"SucceededAt\":\"1764237248291\",\"PerformanceDuration\":\"92\",\"Latency\":\"4195\",\"Result\":\"true\"}'),(253,85,'Enqueued','Triggered by recurring job scheduler','2025-11-27 09:57:34.784243','{\"EnqueuedAt\":\"1764237454782\",\"Queue\":\"workflow\"}'),(254,85,'Processing',NULL,'2025-11-27 09:57:38.154432','{\"StartedAt\":\"1764237458142\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3c7292c8-a7c0-42fb-8967-71dfdeabd467\"}'),(255,85,'Succeeded',NULL,'2025-11-27 09:57:38.232418','{\"SucceededAt\":\"1764237458215\",\"PerformanceDuration\":\"35\",\"Latency\":\"3425\",\"Result\":\"true\"}'),(256,86,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:00:14.290697','{\"EnqueuedAt\":\"1764237614289\",\"Queue\":\"reminder\"}'),(257,87,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:00:14.348133','{\"EnqueuedAt\":\"1764237614346\",\"Queue\":\"emailnotification\"}'),(258,88,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:00:14.406116','{\"EnqueuedAt\":\"1764237614405\",\"Queue\":\"documentindex\"}'),(259,89,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:00:14.466217','{\"EnqueuedAt\":\"1764237614463\",\"Queue\":\"workflow\"}'),(260,86,'Processing',NULL,'2025-11-27 10:00:23.208383','{\"StartedAt\":\"1764237623201\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b4af9158-7b6c-4a2e-9c5b-883a1968cbd8\"}'),(261,87,'Processing',NULL,'2025-11-27 10:00:23.252376','{\"StartedAt\":\"1764237623244\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a6f6d8b-0e2b-4ba9-a572-e78a53d77edd\"}'),(262,86,'Succeeded',NULL,'2025-11-27 10:00:23.275941','{\"SucceededAt\":\"1764237623244\",\"PerformanceDuration\":\"17\",\"Latency\":\"8959\",\"Result\":\"true\"}'),(263,87,'Succeeded',NULL,'2025-11-27 10:00:23.303015','{\"SucceededAt\":\"1764237623290\",\"PerformanceDuration\":\"18\",\"Latency\":\"8951\",\"Result\":\"true\"}'),(264,88,'Processing',NULL,'2025-11-27 10:00:23.334080','{\"StartedAt\":\"1764237623324\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b4af9158-7b6c-4a2e-9c5b-883a1968cbd8\"}'),(265,89,'Processing',NULL,'2025-11-27 10:00:23.375390','{\"StartedAt\":\"1764237623338\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3c7292c8-a7c0-42fb-8967-71dfdeabd467\"}'),(266,88,'Succeeded',NULL,'2025-11-27 10:00:23.397166','{\"SucceededAt\":\"1764237623383\",\"PerformanceDuration\":\"20\",\"Latency\":\"8980\",\"Result\":\"false\"}'),(267,89,'Succeeded',NULL,'2025-11-27 10:00:23.444667','{\"SucceededAt\":\"1764237623430\",\"PerformanceDuration\":\"30\",\"Latency\":\"8960\",\"Result\":\"true\"}'),(268,90,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:03:44.987144','{\"EnqueuedAt\":\"1764237824985\",\"Queue\":\"workflow\"}'),(269,90,'Processing',NULL,'2025-11-27 10:03:53.480199','{\"StartedAt\":\"1764237833444\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"3eb92668-8a58-4a4d-909c-cbc736f98618\"}'),(270,90,'Succeeded',NULL,'2025-11-27 10:03:54.078850','{\"SucceededAt\":\"1764237834060\",\"PerformanceDuration\":\"514\",\"Latency\":\"8660\",\"Result\":\"true\"}'),(271,91,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:06:00.674942','{\"EnqueuedAt\":\"1764237960673\",\"Queue\":\"workflow\"}'),(272,91,'Processing',NULL,'2025-11-27 10:06:08.531918','{\"StartedAt\":\"1764237968508\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a6f6d8b-0e2b-4ba9-a572-e78a53d77edd\"}'),(273,91,'Succeeded',NULL,'2025-11-27 10:06:08.679454','{\"SucceededAt\":\"1764237968655\",\"PerformanceDuration\":\"53\",\"Latency\":\"8025\",\"Result\":\"true\"}'),(274,92,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:09:55.775600','{\"EnqueuedAt\":\"1764238195774\",\"Queue\":\"workflow\"}'),(275,92,'Processing',NULL,'2025-11-27 10:10:08.941446','{\"StartedAt\":\"1764238208910\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"920bd413-0e2b-4066-a64a-723b5a9ba9dc\"}'),(276,92,'Succeeded',NULL,'2025-11-27 10:10:09.095489','{\"SucceededAt\":\"1764238209074\",\"PerformanceDuration\":\"56\",\"Latency\":\"13336\",\"Result\":\"true\"}'),(277,93,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:10:11.096973','{\"EnqueuedAt\":\"1764238211095\",\"Queue\":\"reminder\"}'),(278,93,'Processing',NULL,'2025-11-27 10:10:23.958015','{\"StartedAt\":\"1764238223924\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"2a6f6d8b-0e2b-4ba9-a572-e78a53d77edd\"}'),(279,93,'Succeeded',NULL,'2025-11-27 10:10:24.106551','{\"SucceededAt\":\"1764238224075\",\"PerformanceDuration\":\"53\",\"Latency\":\"13037\",\"Result\":\"true\"}'),(280,94,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:12:26.608207','{\"EnqueuedAt\":\"1764238346606\",\"Queue\":\"workflow\"}'),(281,94,'Processing',NULL,'2025-11-27 10:12:38.983287','{\"StartedAt\":\"1764238358975\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"fa0eb52a-7b77-4f2e-9778-f910e25024ad\"}'),(282,94,'Succeeded',NULL,'2025-11-27 10:12:39.028953','{\"SucceededAt\":\"1764238359018\",\"PerformanceDuration\":\"17\",\"Latency\":\"12421\",\"Result\":\"true\"}'),(283,95,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:15:15.643781','{\"EnqueuedAt\":\"1764238515643\",\"Queue\":\"emailnotification\"}'),(284,96,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:15:15.693324','{\"EnqueuedAt\":\"1764238515692\",\"Queue\":\"documentindex\"}'),(285,97,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:15:15.742819','{\"EnqueuedAt\":\"1764238515742\",\"Queue\":\"workflow\"}'),(286,95,'Processing',NULL,'2025-11-27 10:15:24.009741','{\"StartedAt\":\"1764238524003\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b4af9158-7b6c-4a2e-9c5b-883a1968cbd8\"}'),(287,95,'Succeeded',NULL,'2025-11-27 10:15:24.049794','{\"SucceededAt\":\"1764238524042\",\"PerformanceDuration\":\"17\",\"Latency\":\"8403\",\"Result\":\"true\"}'),(288,96,'Processing',NULL,'2025-11-27 10:15:24.086765','{\"StartedAt\":\"1764238524078\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"b4af9158-7b6c-4a2e-9c5b-883a1968cbd8\"}'),(289,97,'Processing',NULL,'2025-11-27 10:15:24.093422','{\"StartedAt\":\"1764238524084\",\"ServerId\":\"k3lv1n-latitude-3379:20713:44ac5949-b290-44f5-9df6-23ba79b0cfee\",\"WorkerId\":\"fa0eb52a-7b77-4f2e-9778-f910e25024ad\"}'),(290,96,'Succeeded',NULL,'2025-11-27 10:15:24.142556','{\"SucceededAt\":\"1764238524132\",\"PerformanceDuration\":\"22\",\"Latency\":\"8442\",\"Result\":\"false\"}'),(291,97,'Succeeded',NULL,'2025-11-27 10:15:24.148592','{\"SucceededAt\":\"1764238524136\",\"PerformanceDuration\":\"21\",\"Latency\":\"8394\",\"Result\":\"true\"}'),(292,98,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:18:52.750324','{\"EnqueuedAt\":\"1764238732724\",\"Queue\":\"workflow\"}'),(293,98,'Processing',NULL,'2025-11-27 10:19:07.558432','{\"StartedAt\":\"1764238747529\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"377160c5-089c-4898-94e2-76af7f6f91ad\"}'),(294,98,'Succeeded',NULL,'2025-11-27 10:19:08.594439','{\"SucceededAt\":\"1764238748573\",\"PerformanceDuration\":\"992\",\"Latency\":\"14904\",\"Result\":\"true\"}'),(295,99,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:20:52.954390','{\"EnqueuedAt\":\"1764238852953\",\"Queue\":\"reminder\"}'),(296,99,'Processing',NULL,'2025-11-27 10:20:53.672324','{\"StartedAt\":\"1764238853660\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"377160c5-089c-4898-94e2-76af7f6f91ad\"}'),(297,99,'Succeeded',NULL,'2025-11-27 10:20:53.864037','{\"SucceededAt\":\"1764238853848\",\"PerformanceDuration\":\"156\",\"Latency\":\"762\",\"Result\":\"true\"}'),(298,100,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:21:08.996896','{\"EnqueuedAt\":\"1764238868995\",\"Queue\":\"workflow\"}'),(299,100,'Processing',NULL,'2025-11-27 10:21:22.557191','{\"StartedAt\":\"1764238882549\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"8d004b61-425c-4a77-a6e6-2b1730296a8a\"}'),(300,100,'Succeeded',NULL,'2025-11-27 10:21:22.611817','{\"SucceededAt\":\"1764238882600\",\"PerformanceDuration\":\"26\",\"Latency\":\"13606\",\"Result\":\"true\"}'),(301,101,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:25:09.283328','{\"EnqueuedAt\":\"1764239109282\",\"Queue\":\"workflow\"}'),(302,101,'Processing',NULL,'2025-11-27 10:25:22.603606','{\"StartedAt\":\"1764239122594\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"3215e170-de37-497c-975d-2c54cbf90059\"}'),(303,101,'Succeeded',NULL,'2025-11-27 10:25:22.719745','{\"SucceededAt\":\"1764239122702\",\"PerformanceDuration\":\"78\",\"Latency\":\"13365\",\"Result\":\"true\"}'),(304,102,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:27:24.480344','{\"EnqueuedAt\":\"1764239244479\",\"Queue\":\"workflow\"}'),(305,102,'Processing',NULL,'2025-11-27 10:27:37.629415','{\"StartedAt\":\"1764239257620\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"615643b9-a07b-4304-827b-4601bbc8b974\"}'),(306,102,'Succeeded',NULL,'2025-11-27 10:27:37.687339','{\"SucceededAt\":\"1764239257675\",\"PerformanceDuration\":\"26\",\"Latency\":\"13195\",\"Result\":\"true\"}'),(307,103,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:30:06.438504','{\"EnqueuedAt\":\"1764239406436\",\"Queue\":\"reminder\"}'),(308,104,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:30:06.514651','{\"EnqueuedAt\":\"1764239406512\",\"Queue\":\"emailnotification\"}'),(309,105,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:30:06.578433','{\"EnqueuedAt\":\"1764239406576\",\"Queue\":\"documentindex\"}'),(310,106,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:30:06.643410','{\"EnqueuedAt\":\"1764239406640\",\"Queue\":\"workflow\"}'),(311,103,'Processing',NULL,'2025-11-27 10:30:07.679110','{\"StartedAt\":\"1764239407666\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"80cadcbd-3d17-4bbd-b83b-60c4cc7518fc\"}'),(312,104,'Processing',NULL,'2025-11-27 10:30:07.735317','{\"StartedAt\":\"1764239407674\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"a39705de-8f9b-4073-a6a8-e00815473339\"}'),(313,106,'Processing',NULL,'2025-11-27 10:30:07.767485','{\"StartedAt\":\"1764239407676\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"6a04ab69-bd5b-456d-9faa-036c236feb9c\"}'),(314,105,'Processing',NULL,'2025-11-27 10:30:07.809721','{\"StartedAt\":\"1764239407675\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"594558b1-bfb4-45bc-9fcb-130ea9a9f449\"}'),(315,103,'Succeeded',NULL,'2025-11-27 10:30:07.815746','{\"SucceededAt\":\"1764239407800\",\"PerformanceDuration\":\"88\",\"Latency\":\"1302\",\"Result\":\"true\"}'),(316,106,'Succeeded',NULL,'2025-11-27 10:30:07.856591','{\"SucceededAt\":\"1764239407836\",\"PerformanceDuration\":\"41\",\"Latency\":\"1182\",\"Result\":\"true\"}'),(317,105,'Succeeded',NULL,'2025-11-27 10:30:07.953052','{\"SucceededAt\":\"1764239407934\",\"PerformanceDuration\":\"105\",\"Latency\":\"1278\",\"Result\":\"false\"}'),(318,104,'Succeeded',NULL,'2025-11-27 10:30:07.983051','{\"SucceededAt\":\"1764239407963\",\"PerformanceDuration\":\"206\",\"Latency\":\"1278\",\"Result\":\"true\"}'),(319,107,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:33:36.932136','{\"EnqueuedAt\":\"1764239616931\",\"Queue\":\"workflow\"}'),(320,107,'Processing',NULL,'2025-11-27 10:33:37.751166','{\"StartedAt\":\"1764239617742\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"0b814248-3719-42e2-a586-b2e63e826c07\"}'),(321,107,'Succeeded',NULL,'2025-11-27 10:33:37.795174','{\"SucceededAt\":\"1764239617785\",\"PerformanceDuration\":\"17\",\"Latency\":\"860\",\"Result\":\"true\"}'),(322,108,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:36:09.421439','{\"EnqueuedAt\":\"1764239769420\",\"Queue\":\"workflow\"}'),(323,108,'Processing',NULL,'2025-11-27 10:36:22.850826','{\"StartedAt\":\"1764239782843\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"33ecd3db-4210-4315-b4e3-5cb63351afd5\"}'),(324,108,'Succeeded',NULL,'2025-11-27 10:36:22.904403','{\"SucceededAt\":\"1764239782891\",\"PerformanceDuration\":\"22\",\"Latency\":\"13481\",\"Result\":\"true\"}'),(325,109,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:40:09.732727','{\"EnqueuedAt\":\"1764240009731\",\"Queue\":\"reminder\"}'),(326,110,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:40:09.789622','{\"EnqueuedAt\":\"1764240009789\",\"Queue\":\"workflow\"}'),(327,110,'Processing',NULL,'2025-11-27 10:40:22.915236','{\"StartedAt\":\"1764240022894\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"615643b9-a07b-4304-827b-4601bbc8b974\"}'),(328,109,'Processing',NULL,'2025-11-27 10:40:22.918111','{\"StartedAt\":\"1764240022884\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"11d1d948-2cc1-48c8-a478-6cc903523843\"}'),(329,110,'Succeeded',NULL,'2025-11-27 10:40:22.975703','{\"SucceededAt\":\"1764240022961\",\"PerformanceDuration\":\"22\",\"Latency\":\"13178\",\"Result\":\"true\"}'),(330,109,'Succeeded',NULL,'2025-11-27 10:40:23.000889','{\"SucceededAt\":\"1764240022990\",\"PerformanceDuration\":\"47\",\"Latency\":\"13236\",\"Result\":\"true\"}'),(331,111,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:42:09.970599','{\"EnqueuedAt\":\"1764240129969\",\"Queue\":\"workflow\"}'),(332,111,'Processing',NULL,'2025-11-27 10:42:22.945604','{\"StartedAt\":\"1764240142937\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"cd759922-44e7-442c-8d53-a124f157d9d8\"}'),(333,111,'Succeeded',NULL,'2025-11-27 10:42:22.987812','{\"SucceededAt\":\"1764240142979\",\"PerformanceDuration\":\"17\",\"Latency\":\"13016\",\"Result\":\"true\"}'),(334,112,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:45:13.182793','{\"EnqueuedAt\":\"1764240313181\",\"Queue\":\"emailnotification\"}'),(335,113,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:45:13.235771','{\"EnqueuedAt\":\"1764240313235\",\"Queue\":\"documentindex\"}'),(336,114,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:45:13.287864','{\"EnqueuedAt\":\"1764240313287\",\"Queue\":\"workflow\"}'),(337,112,'Processing',NULL,'2025-11-27 10:45:22.991928','{\"StartedAt\":\"1764240322980\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"0b814248-3719-42e2-a586-b2e63e826c07\"}'),(338,114,'Processing',NULL,'2025-11-27 10:45:23.005059','{\"StartedAt\":\"1764240322992\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"cd118224-bd8f-4e0b-ae71-fc60877e88e5\"}'),(339,113,'Processing',NULL,'2025-11-27 10:45:23.022673','{\"StartedAt\":\"1764240322989\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"8d004b61-425c-4a77-a6e6-2b1730296a8a\"}'),(340,112,'Succeeded',NULL,'2025-11-27 10:45:23.062891','{\"SucceededAt\":\"1764240323046\",\"PerformanceDuration\":\"22\",\"Latency\":\"9863\",\"Result\":\"true\"}'),(341,114,'Succeeded',NULL,'2025-11-27 10:45:23.075186','{\"SucceededAt\":\"1764240323060\",\"PerformanceDuration\":\"29\",\"Latency\":\"9767\",\"Result\":\"true\"}'),(342,113,'Succeeded',NULL,'2025-11-27 10:45:23.093454','{\"SucceededAt\":\"1764240323079\",\"PerformanceDuration\":\"26\",\"Latency\":\"9842\",\"Result\":\"false\"}'),(343,115,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:48:44.126155','{\"EnqueuedAt\":\"1764240524124\",\"Queue\":\"workflow\"}'),(344,115,'Processing',NULL,'2025-11-27 10:48:53.086532','{\"StartedAt\":\"1764240533045\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"093b938a-a32f-4489-8791-338553ac7b2f\"}'),(345,115,'Succeeded',NULL,'2025-11-27 10:48:53.211504','{\"SucceededAt\":\"1764240533185\",\"PerformanceDuration\":\"44\",\"Latency\":\"9576\",\"Result\":\"true\"}'),(346,116,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:50:44.807678','{\"EnqueuedAt\":\"1764240644806\",\"Queue\":\"reminder\"}'),(347,116,'Processing',NULL,'2025-11-27 10:50:53.111513','{\"StartedAt\":\"1764240653089\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"3215e170-de37-497c-975d-2c54cbf90059\"}'),(348,116,'Succeeded',NULL,'2025-11-27 10:50:53.289477','{\"SucceededAt\":\"1764240653251\",\"PerformanceDuration\":\"61\",\"Latency\":\"8477\",\"Result\":\"true\"}'),(349,117,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:51:39.334707','{\"EnqueuedAt\":\"1764240699333\",\"Queue\":\"workflow\"}'),(350,117,'Processing',NULL,'2025-11-27 10:51:53.275881','{\"StartedAt\":\"1764240713192\",\"ServerId\":\"k3lv1n-latitude-3379:39435:745b6768-ca24-4061-a877-8469ba217d3d\",\"WorkerId\":\"80cadcbd-3d17-4bbd-b83b-60c4cc7518fc\"}'),(351,117,'Succeeded',NULL,'2025-11-27 10:51:53.974558','{\"SucceededAt\":\"1764240713889\",\"PerformanceDuration\":\"194\",\"Latency\":\"14452\",\"Result\":\"true\"}'),(352,118,'Enqueued','Triggered by recurring job scheduler','2025-11-27 10:59:11.675158','{\"EnqueuedAt\":\"1764241151644\",\"Queue\":\"workflow\"}'),(353,118,'Processing',NULL,'2025-11-27 10:59:26.540610','{\"StartedAt\":\"1764241166510\",\"ServerId\":\"k3lv1n-latitude-3379:45684:399789f4-aa35-4a22-a52e-3a87c7cc47f1\",\"WorkerId\":\"29ad121f-5d76-4739-a11e-771d64b5a7df\"}'),(354,118,'Succeeded',NULL,'2025-11-27 10:59:27.407155','{\"SucceededAt\":\"1764241167387\",\"PerformanceDuration\":\"822\",\"Latency\":\"14969\",\"Result\":\"true\"}'),(355,119,'Enqueued','Triggered by recurring job scheduler','2025-11-27 11:00:30.614976','{\"EnqueuedAt\":\"1764241230614\",\"Queue\":\"reminder\"}'),(356,120,'Enqueued','Triggered by recurring job scheduler','2025-11-27 11:00:30.778291','{\"EnqueuedAt\":\"1764241230776\",\"Queue\":\"emailnotification\"}'),(357,121,'Enqueued','Triggered by recurring job scheduler','2025-11-27 11:00:30.956213','{\"EnqueuedAt\":\"1764241230954\",\"Queue\":\"documentindex\"}'),(358,122,'Enqueued','Triggered by recurring job scheduler','2025-11-27 11:00:31.142548','{\"EnqueuedAt\":\"1764241231141\",\"Queue\":\"workflow\"}'),(359,119,'Processing',NULL,'2025-11-27 11:00:41.561592','{\"StartedAt\":\"1764241241526\",\"ServerId\":\"k3lv1n-latitude-3379:45684:399789f4-aa35-4a22-a52e-3a87c7cc47f1\",\"WorkerId\":\"aff2fa78-1014-44d9-9981-6bd1d6f254a3\"}'),(360,120,'Processing',NULL,'2025-11-27 11:00:41.587798','{\"StartedAt\":\"1764241241558\",\"ServerId\":\"k3lv1n-latitude-3379:45684:399789f4-aa35-4a22-a52e-3a87c7cc47f1\",\"WorkerId\":\"203085d2-734d-4cda-ab89-9dec9dda6666\"}'),(361,121,'Processing',NULL,'2025-11-27 11:00:41.630732','{\"StartedAt\":\"1764241241581\",\"ServerId\":\"k3lv1n-latitude-3379:45684:399789f4-aa35-4a22-a52e-3a87c7cc47f1\",\"WorkerId\":\"fd22c271-47d9-40b5-b606-4feb4cbc6f9b\"}'),(362,122,'Processing',NULL,'2025-11-27 11:00:41.653677','{\"StartedAt\":\"1764241241621\",\"ServerId\":\"k3lv1n-latitude-3379:45684:399789f4-aa35-4a22-a52e-3a87c7cc47f1\",\"WorkerId\":\"ed9ae208-82a2-479b-8939-9be871420180\"}'),(363,119,'Succeeded',NULL,'2025-11-27 11:00:41.898476','{\"SucceededAt\":\"1764241241869\",\"PerformanceDuration\":\"185\",\"Latency\":\"11158\",\"Result\":\"true\"}'),(364,120,'Succeeded',NULL,'2025-11-27 11:00:41.914852','{\"SucceededAt\":\"1764241241882\",\"PerformanceDuration\":\"181\",\"Latency\":\"10997\",\"Result\":\"true\"}'),(365,121,'Succeeded',NULL,'2025-11-27 11:00:41.960633','{\"SucceededAt\":\"1764241241922\",\"PerformanceDuration\":\"175\",\"Latency\":\"10881\",\"Result\":\"false\"}'),(366,122,'Succeeded',NULL,'2025-11-27 11:00:42.002909','{\"SucceededAt\":\"1764241241952\",\"PerformanceDuration\":\"187\",\"Latency\":\"10716\",\"Result\":\"true\"}'),(367,123,'Enqueued','Triggered by recurring job scheduler','2025-11-27 11:03:31.520156','{\"EnqueuedAt\":\"1764241411518\",\"Queue\":\"workflow\"}'),(368,123,'Processing',NULL,'2025-11-27 11:03:41.659377','{\"StartedAt\":\"1764241421652\",\"ServerId\":\"k3lv1n-latitude-3379:45684:399789f4-aa35-4a22-a52e-3a87c7cc47f1\",\"WorkerId\":\"9c169b40-55d9-4815-bb1a-251b9fd1ab63\"}'),(369,123,'Succeeded',NULL,'2025-11-27 11:03:41.718173','{\"SucceededAt\":\"1764241421708\",\"PerformanceDuration\":\"32\",\"Latency\":\"10184\",\"Result\":\"true\"}'),(370,124,'Enqueued','Triggered by recurring job scheduler','2025-11-27 11:06:04.282569','{\"EnqueuedAt\":\"1764241564280\",\"Queue\":\"workflow\"}'),(371,124,'Processing',NULL,'2025-11-27 11:06:11.688920','{\"StartedAt\":\"1764241571680\",\"ServerId\":\"k3lv1n-latitude-3379:45684:399789f4-aa35-4a22-a52e-3a87c7cc47f1\",\"WorkerId\":\"98299618-d556-413c-b1a3-7501ca902c2e\"}'),(372,124,'Succeeded',NULL,'2025-11-27 11:06:11.735318','{\"SucceededAt\":\"1764241571724\",\"PerformanceDuration\":\"18\",\"Latency\":\"7449\",\"Result\":\"true\"}');
/*!40000 ALTER TABLE `HangfireState` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `LoginAudits`
--

DROP TABLE IF EXISTS `LoginAudits`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `LoginAudits` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `LoginTime` datetime NOT NULL,
  `RemoteIP` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Provider` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Latitude` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Longitude` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `LoginAudits`
--

LOCK TABLES `LoginAudits` WRITE;
/*!40000 ALTER TABLE `LoginAudits` DISABLE KEYS */;
INSERT INTO `LoginAudits` VALUES ('041dba51-63d5-4365-8cb1-91d5fab10e67','admin@gmail.com','2025-11-27 10:30:45','127.0.0.1','Success',NULL,NULL,NULL),('0603762b-57e6-4bd2-bc6c-461d34a9fbdd','admin@gmail.com','2025-11-27 08:49:39','127.0.0.1','Error',NULL,'-1.703936','37.1785728'),('0bda055e-1466-43de-9c08-1ff431ca765a','admin@gmail.com','2025-11-27 08:50:09','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('19a56884-a1d1-45c7-add6-4913e77e5e89','admin@gmail.com','2025-11-27 08:50:11','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('21152896-2e41-4521-a78f-e33e9e2750dd','admin@gmail.com','2025-11-27 07:57:26','127.0.0.1','Success',NULL,'-1.7104896','37.1785728'),('2fdc3c91-0d33-461a-a967-fcd58399c095','admin@gmail.com','2025-11-27 08:50:11','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('57fc8160-17dc-43eb-bbba-3ad3bec6839d','admin@gmail.com','2025-11-27 07:48:38','::1','Error',NULL,NULL,NULL),('66775a39-1243-482f-b5b7-cdbd6c18036c','admin@gmail.com','2025-11-27 08:49:51','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('6a944571-c803-4ea9-9327-6a320b58fd47','admin@gmail.com','2025-11-27 07:29:50','127.0.0.1','Error',NULL,'-1.7104896','37.1785728'),('7c463a8f-aa5c-4f56-bf3e-dfb5100b2b47','admin@gmail.com','2025-11-27 08:50:10','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('7e5d685c-2a74-4706-af5b-5eef2fdc9bbb','admin@gmail.com','2025-11-27 08:50:08','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('9a5df920-ca4f-4d77-b469-0d14d5d66f7c','admin@gmail.com','2025-11-27 07:29:36','127.0.0.1','Error',NULL,'-1.7104896','37.1785728'),('9ccf71fc-681c-48b4-9ccd-623b96fe5246','admin@gmail.com','2025-11-27 08:50:11','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('9e887b13-5464-47f5-8ba2-d585959d2f64','admin@gmail.com','2025-11-27 07:50:38','::1','Success',NULL,NULL,NULL),('a0123a4f-ff1d-4e0f-b151-63b26987ffd1','admin@gmail.com','2025-11-27 08:50:09','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('a9af351e-0b4a-4672-a4f9-498b55c725e8','admin@gmail.com','2025-11-27 08:50:10','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('aa2614e1-4160-409f-aa1a-54a07a257f64','admin@gmail.com','2025-11-27 07:53:53','127.0.0.1','Error',NULL,'-1.7104896','37.1785728'),('ad1f8350-08e3-4bb2-b484-196d811f16c1','admin@gmail.com','2025-11-27 09:09:15','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('aeda53da-c09e-4a72-a0d4-428b20248fb7','admin@gmail.com','2025-11-27 09:07:39','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('c2d05689-acfe-40c6-a51e-4f1fc2804bc1','admin@gmail.com','2025-11-27 08:50:05','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('d5983c93-246a-4e3d-9c0d-810d221691da','admin@gmail.com','2025-11-27 08:50:10','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('dc083b0f-3cb6-4458-8cf7-6c834539af49','admin@gmail.com','2025-11-27 08:50:10','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('ee7800b5-121e-49ae-a5a7-7b10c7eeb85f','admin@gmail.com','2025-11-27 08:56:14','127.0.0.1','Success',NULL,NULL,NULL),('f8994275-437c-4912-a880-b89cf426c366','admin@gmail.com','2025-11-27 08:50:11','127.0.0.1','Success',NULL,'-1.703936','37.1785728'),('fbdc68f9-340f-466c-bca5-77d81ef8ce39','admin@gmail.com','2025-11-27 08:50:12','127.0.0.1','Success',NULL,'-1.703936','37.1785728');
/*!40000 ALTER TABLE `LoginAudits` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `MatTableSettings`
--

DROP TABLE IF EXISTS `MatTableSettings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `MatTableSettings` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ScreenName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `SettingsJson` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `MatTableSettings`
--

LOCK TABLES `MatTableSettings` WRITE;
/*!40000 ALTER TABLE `MatTableSettings` DISABLE KEYS */;
/*!40000 ALTER TABLE `MatTableSettings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `NLog`
--

DROP TABLE IF EXISTS `NLog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `NLog` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `MachineName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Logged` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Level` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Message` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Logger` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Properties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Callsite` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Exception` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `NLog`
--

LOCK TABLES `NLog` WRITE;
/*!40000 ALTER TABLE `NLog` DISABLE KEYS */;
/*!40000 ALTER TABLE `NLog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `PageActions`
--

DROP TABLE IF EXISTS `PageActions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `PageActions` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Order` int NOT NULL,
  `PageId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Code` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_PageActions_PageId` (`PageId`),
  CONSTRAINT `FK_PageActions_Screens_PageId` FOREIGN KEY (`PageId`) REFERENCES `Screens` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `PageActions`
--

LOCK TABLES `PageActions` WRITE;
/*!40000 ALTER TABLE `PageActions` DISABLE KEYS */;
INSERT INTO `PageActions` VALUES ('00e99d70-a221-47e6-b56f-cc19e154b6c6','Restore Document',4,'8130904f-4e5b-47d4-8acf-c9cff36d48a4','Restore_Document'),('01CE3C47-67A7-4FAE-821A-494BFF0D46EF','Add Comment',1,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Add_Comment'),('044ceb92-87fc-41a5-93a7-ffaf096db766','Edit Role',3,'090ea443-01c7-4638-a194-ad3416a5ea7a','Edit_Role'),('07767bf0-ca74-423b-9ad2-0e2a0bd1087b','Add Signature',3,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','All_Add_Signature'),('0a74c66d-10ec-43a8-ab35-7b45ce0d6092','Download Document',3,'8130904f-4e5b-47d4-8acf-c9cff36d48a4','Archive_Download_Document'),('14f96237-e4ad-4cdb-af33-dc0f98abe121','View Prompt Templates',7,'479b5173-369e-487a-817d-651d77782590','View_Prompt_Templates'),('197C97B0-BD33-4E2E-8C1E-BCCFD1C65FBD','Delete Comment',8,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Delete_Comment'),('1a0a3737-ee82-46dc-a1b1-8bbc3aee23f6','Create SMTP Setting',1,'2e3c07a4-fcac-4303-ae47-0d0f796403c9','Create_SMTP_Setting'),('1a3346d9-3c8d-4ae0-9416-db9a157d20f2','Delete Role',2,'090ea443-01c7-4638-a194-ad3416a5ea7a','Delete_Role'),('1c0a486d-53a9-4239-97ae-233366a63646','Rejected File Request Documents',6,'ffee08a0-35e0-485a-a335-e455fb59e344','Rejected_File_Request_Documents'),('1d831629-f1b9-4c47-b70d-97893908fb46','General Settings',1,'28e6dbe7-1d17-4875-b96f-8757870d20af','General_Settings'),('1eb6b640-de77-4d96-8487-222367b1233d','Workflows',1,'efe7d58a-1c33-4f2a-9b7b-3e1dff6b5664','Workflows'),('23ddf867-056f-425b-99ed-d298bbd2d80f','Edit User',5,'324bdc51-d71f-4f80-9f28-a30e8aae4009','Edit_User'),('2A9169A8-A46E-4D61-94FC-AA8A9ADC459A','Manage Page Helper',1,'73DF018E-77B1-42FB-B8D7-D7A09836E453','Manage_Page_Helper'),('2c5ac3ce-a7e8-4feb-80b7-8fe10d412631','Delete Clients',2,'dc075340-895b-41c0-93da-1767f097a64b','Delete_Clients'),('2e159fd9-df31-400f-9dde-88bdc34bf42f','Add Signature',4,'FC97DC8F-B4DA-46B1-A179-AB206D8B7EFD','Assigned_Add_Signature'),('2f4259e3-6f34-4f8d-9189-b71e5efeb8e1','Delete Allow File Extensions',2,'c6d248e0-f702-4334-b0b8-0c403a82715f','Delete_Allow_File_Extensions'),('35746e9b-7bb3-4272-b0f3-5abde75fe6cd','Bulk Document Uploads',1,'eb342fd3-836f-437c-9b6b-b7e4188fb49c','Bulk_Document_Uploads'),('35b754c5-0e48-4788-a71c-4de13a8fe449','Delete Document Meta Tags',2,'a51a1261-aa38-4c07-a513-38c8e619d141','Delete_Document_Meta_Tags'),('3729cbe2-302d-4096-934e-b4cad63711e3','Start Workflow',17,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','All_Start_Workflow'),('37653d9f-73d2-459f-ba17-e678944c0703','Add Prompt Templates',1,'479b5173-369e-487a-817d-651d77782590','Add_Prompt_Templates'),('38862aeb-f2d6-4256-b506-3298f12d0f03','View File Request',7,'ffee08a0-35e0-485a-a335-e455fb59e344','View_File_Request'),('39836641-143c-4e37-bc18-12af4eb95f01','AI Document Generator',2,'479b5173-369e-487a-817d-651d77782590','AI_Document_Generator'),('3b11f492-781a-4640-86ec-4382bc46d7d0','OCR Content Extractor',1,'f4a940bd-8f81-4b5c-abd1-0ddb8083c4da','OCR_Content_Extractor'),('3c7ea13e-e6be-4035-90cd-9fdf793ce89f','Delete AI Document Generator',3,'479b5173-369e-487a-817d-651d77782590','Delete_AI_Document_Generator'),('42794D41-EF8A-4F8E-B0AD-1FCEB50B3E2E','Deep Search',2,'D6B63A2E-F7DB-4F83-B5DD-96A000D9A789','Deep_Search'),('44195689-7037-4bce-9e5f-b7de96c4fba8','Remove Share Folder',16,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Remove_Share_Folder'),('4b60217a-a360-40d2-8641-d7d336a1a1e4','View Allow File Extensions',4,'c6d248e0-f702-4334-b0b8-0c403a82715f','View_Allow_File_Extensions'),('4cd7059f-53b5-4ec2-be6b-78cdab96eb70','Edit Document Meta Tags',3,'a51a1261-aa38-4c07-a513-38c8e619d141','Edit_Document_Meta_Tags'),('4de6055c-5f81-44d8-aee2-b966fc442263','Delete SMTP Setting',2,'2e3c07a4-fcac-4303-ae47-0d0f796403c9','Delete_SMTP_Setting'),('51c88956-ea5a-4934-96ba-fd09905a1b0a','View Dashboard',1,'42e44f15-8e33-423a-ad7f-17edc23d6dd3','View_Dashboard'),('5b5e4d58-cdfc-4e4b-8083-9b497230af4b','Delete Document',2,'8130904f-4e5b-47d4-8acf-c9cff36d48a4','Delete_Document'),('5c7f8415-59ac-4ae7-a622-a5729aaac8ab','Approved File Request Documents',3,'ffee08a0-35e0-485a-a335-e455fb59e344','Approved_File_Request_Documents'),('5d4859df-2354-4e77-b3c2-f9eb85f7836d','Current Workflow',1,'a676aa9b-8ccb-4608-8e21-2cdd9a37fd99','Current_Workflow'),('5d5e0edc-e14f-48ad-bf1d-3dfbd9ac55aa','Create Role',1,'090ea443-01c7-4638-a194-ad3416a5ea7a','Create_Role'),('5dbdba2a-17f7-48f9-989c-52322fd24d8c','Create Shareable Link',1,'8130904f-4e5b-47d4-8acf-c9cff36d48a4','Archive_Create_Shareable_Link'),('5ea98fd6-f86f-462b-ba4a-c2a7cadcf122','Delete Prompt Templates',4,'479b5173-369e-487a-817d-651d77782590','Delete_Prompt_Templates'),('5f3963ab-ceb2-443e-94b6-9ef0752a2d2e','View Document Meta Tags',4,'a51a1261-aa38-4c07-a513-38c8e619d141','View_Document_Meta_Tags'),('616254DE-2EA2-46E3-B7C3-4596D6180144','Add Comment',1,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','ALL_Add_Comment'),('617c390b-99c7-4a22-bd16-154aecc63169','Send Email',5,'8130904f-4e5b-47d4-8acf-c9cff36d48a4','Archive_Send_Email'),('61C15E32-28A9-4DCE-8CDE-5CA8325F5F04','Manage Company Settings',1,'669C82F1-0DE0-459C-B62A-83A9614259E4','Manage_Company_Settings'),('63B2165E-CB49-4F10-B504-082F618B76F0','View version history',20,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','all_view_version_history'),('65dfed53-7855-46f5-ab93-3629fc68ea71','View Document Audit Trail',1,'2396f81c-f8b5-49ac-88d1-94ed57333f49','View_Document_Audit_Trail'),('670fc04b-7367-4984-8aeb-037705d7bfc8','Archive Document',5,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Archive_Document'),('6a048b38-5b3a-42b0-83fd-2c4d588d0b2f','Create Reminder',1,'97ff6eb0-39b3-4ddd-acf1-43205d5a9bb3','Create_Reminder'),('6adf6012-0101-48b2-ad54-078d2f7fe96d','View Users',7,'324bdc51-d71f-4f80-9f28-a30e8aae4009','View_Users'),('6E52219E-0B7F-4E57-B7B3-DB688DE17AF0','Create Shareable Link',6,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','ALL_Create_Shareable_Link'),('6fd5cbb9-d1b5-4915-9fa9-ab036a5b26f6','Get Document Summary',10,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','ALL_Get_Document_Summary'),('70826821-6FEE-4BF2-A64D-49CD41E7823D','Create Shareable Link',7,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Create_Shareable_Link'),('723e56ce-a020-452f-8773-2f27e2725f1b','Share Document',21,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Share_Document'),('724b0bb9-d15f-4ad6-bf43-a76b1a393bf6','Delete Category',1,'2502e117-1752-4c6f-8fdb-cb32cb4c1e59','Delete_Category'),('74BDCA6C-DDF5-49A6-9719-F364AB7BEE11','Upload New version',18,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','All_Upload_New_version'),('761032d2-822a-4274-ab85-3b389f5ec252','Share Document',16,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','ALL_Share_Document'),('764568df-718a-4a10-a6b5-a11523b22c19','Restore version',17,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Restore_version'),('77B6DC42-AECE-4FDB-8203-06B61C2DDDB0','Manage Document Status',1,'4513EAE1-373A-4734-928C-8943C3F070BB','Manage_Document_Status'),('7999014d-dfcd-4f2c-b6d8-cf66571d1fb7','View Documents',6,'8130904f-4e5b-47d4-8acf-c9cff36d48a4','View_Documents'),('7b90dcef-ec68-4c75-ab24-c1bf2c365b09','Edit Allow File Extensions',3,'c6d248e0-f702-4334-b0b8-0c403a82715f','Edit_Allow_File_Extensions'),('80071200-5e95-4b91-9334-c1d80a2c9b46','Add Document Meta Tags',1,'a51a1261-aa38-4c07-a513-38c8e619d141','Add_Document_Meta_Tags'),('8105bd15-f1e3-435b-be45-38cf8dea8800','View version history',7,'8130904f-4e5b-47d4-8acf-c9cff36d48a4','Archive_View_version_history'),('82e66203-b6d6-440e-8c9b-733163b574a1','View Clients',4,'dc075340-895b-41c0-93da-1767f097a64b','View_Clients'),('87089dd2-149a-49c4-931c-18b47e08561c','Reset Password',6,'324bdc51-d71f-4f80-9f28-a30e8aae4009','Reset_Password'),('8a2e6320-2aa2-496c-9e72-0cd7be0140df','Allow To See File Request',2,'ffee08a0-35e0-485a-a335-e455fb59e344','Allow_To_See_File_Request'),('8a90c207-7752-4277-83f6-5345ed277d7a','View Roles',4,'090ea443-01c7-4638-a194-ad3416a5ea7a','View_Roles'),('8c98e9a6-8edc-4e4e-8714-37752437f10d','Share Folder',4,'5a5f7cf8-21a6-434a-9330-db91b17d867c','Category_Share_Folder'),('8e82fe1f-8ccd-4cc2-b1ca-1a84dd17a5ab','Create Category',2,'5a5f7cf8-21a6-434a-9330-db91b17d867c','Create_Category'),('8f065fb5-01c7-4dea-ab19-650392338688','Edit SMTP Setting',3,'2e3c07a4-fcac-4303-ae47-0d0f796403c9','Edit_SMTP_Setting'),('930af13c-158e-493a-ba7c-6406587b5286','Archive Document',4,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','All_Archive_Document'),('95322722-6362-41ac-8165-9c64076399fb','Workflow Logs',1,'33c5442e-2844-497c-af00-9cda5a6fc826','Workflow_Logs'),('97ab4ba3-be33-4d8b-9363-86cf013f592c','Add File Request',1,'ffee08a0-35e0-485a-a335-e455fb59e344','Add_File_Request'),('9eac28ed-e662-4c9c-8bf9-8e14d7568afc','Edit File Request',5,'ffee08a0-35e0-485a-a335-e455fb59e344','Edit_File_Request'),('A2148F30-0A60-420E-992A-C93B4B297DF8','View version history',26,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_View_version_history'),('A73CCEF3-DA47-49E8-9944-EBA0F18B2E40','Remove Document Index',3,'D6B63A2E-F7DB-4F83-B5DD-96A000D9A789','Remove_Document_Index'),('aa12fe9c-3388-429c-90f8-070fb01d5868','Remove Share Document',12,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','ALL_Remove_Share_Document'),('B0081952-68C8-4C89-83B1-3D5C97941C19','Restore version',14,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','ALL_Restore_version'),('b1375795-6655-47fc-be67-5569a2fe2517','Archive Folder',1,'da660d2d-450b-4825-9489-d87ef94ad6ff','Assigned_Archive_Folder'),('b13dc77a-32b9-4f48-96de-90539ba688fa','View SMTP Settings',4,'2e3c07a4-fcac-4303-ae47-0d0f796403c9','View_SMTP_Settings'),('B470AE88-E223-45DE-8720-3D4467A5F702','Delete Comment',7,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','ALL_Delete_Comment'),('b4fc0f33-0e9b-4b22-b357-d85125ba8d49','Edit Document',9,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','All_Edit_Document'),('b7d48f9a-c54c-4394-81ce-ea10aba9df87','Create Document',6,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Create_Document'),('bc56d098-f652-430c-be0d-210090a298cb','Delete File Request',4,'ffee08a0-35e0-485a-a335-e455fb59e344','Delete_File_Request'),('bcd3bbf9-fff5-4ece-b82e-35b130406b25','Add Allow File Extensions',1,'c6d248e0-f702-4334-b0b8-0c403a82715f','Add_Allow_File_Extensions'),('c2b9abde-bf77-4887-b18e-4a84d24699de','Share Folder',2,'b6058070-8cca-4bc0-a88a-5596cbd63683','All_Share_Folder'),('c3768706-4767-472e-9bbf-0c515bec6ff4','Archive Folder',1,'b6058070-8cca-4bc0-a88a-5596cbd63683','ALL_Archive_Folder'),('c45ee6ab-20a8-44b2-ae88-8fcbf4fbd54d','Edit Workflow Settings',3,'309f72e8-6e93-43fa-9203-808b030a33f7','Edit_Workflow_Settings'),('c5b79883-882f-4e1f-bbbe-f6ed055484ca','Edit Clients',3,'dc075340-895b-41c0-93da-1767f097a64b','Edit_Clients'),('c8e790f2-6fb6-412a-a2fb-45dc579cae43','Add Clients',1,'dc075340-895b-41c0-93da-1767f097a64b','Add_Clients'),('c9928f1f-0702-4e37-97a7-431e5c9f819c','Delete User',4,'324bdc51-d71f-4f80-9f28-a30e8aae4009','Delete_User'),('cb980805-4de9-45b6-a12d-bb0f91d549cb','Assign Permission',1,'324bdc51-d71f-4f80-9f28-a30e8aae4009','Assign_Permission'),('cc5d7643-e418-492f-bbbd-409a336dbce5','Archive Folder',1,'5a5f7cf8-21a6-434a-9330-db91b17d867c','Category_Archive_Folder'),('cfde1c32-3f74-47e7-b0c8-f7061acac39b','View AI Document Generator',6,'479b5173-369e-487a-817d-651d77782590','View_AI_Document_Generator'),('d1f39f95-d550-474b-96fa-99b097b34c1b','Edit Document',10,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Edit_Document'),('D21077FE-3441-4AD2-BD22-BD13233EE5B2','View Error Logs',1,'42338E4F-05E0-48D1-862A-D977C39D02DF','View_Error_Logs'),('d2ae9a3b-a491-4a07-b155-6401ef11712a','Remove Share Document',15,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Remove_Share_Document'),('d40e679d-a08a-4dde-bca8-e75ebfdbd817','Remove Share Folder',13,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','All_Remove_Share_Folder'),('d53f507b-c73c-435f-a4d0-69fe616b8d80','View Reminders',4,'97ff6eb0-39b3-4ddd-acf1-43205d5a9bb3','View_Reminders'),('d5e8bac8-c173-459b-be1d-394b2b46f2d9','Manage Archive Retention Period',1,'9e58a819-05ce-4226-ad70-31d2897f94be','Manage_Archive_Retention_Period'),('d886ffaa-e26f-4e27-b4e5-c3636f6422cf','View Login Audit Logs',1,'f042bbee-d15f-40fb-b79a-8368f2c2e287','View_Login_Audit_Logs'),('db16125a-e16e-498f-980f-7f8f397dd920','Delete Workflow Settings',2,'309f72e8-6e93-43fa-9203-808b030a33f7','Delete_Workflow_Settings'),('dcba14ed-cb99-44d4-8b4f-53d8f249ed20','Add Reminder',2,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','All_Add_Reminder'),('dcf6f998-7276-4a1c-aba1-8513f2eaebad','Upload New version',25,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Upload_New_version'),('ded2da54-9077-46b4-8d2e-db69890bed25','View Documents',19,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','All_View_Documents'),('df42ffb1-f957-4c63-b07a-c57b1f915ff2','Restore Folder',2,'2502e117-1752-4c6f-8fdb-cb32cb4c1e59','Restore_Folder'),('DF81DECD-4282-4873-9606-AB353FCC4523','Add Reminder',3,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Add_Reminder'),('e1278d04-1e53-4885-b7f3-8dd9786ee8ba','Assign User Role',2,'324bdc51-d71f-4f80-9f28-a30e8aae4009','Assign_User_Role'),('e37e63ee-3aa4-4521-b913-95d48b8c831a','Start Workflow',23,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Start_Workflow'),('e67675a7-cd03-4b28-bd2f-437a813686b0','Edit Category',3,'5a5f7cf8-21a6-434a-9330-db91b17d867c','Edit_Category'),('e945d35f-d3f6-46e7-9723-a62f89b2022e','Send Email',19,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Send_Email'),('e99d8d8b-961c-47ad-85d8-a7b57c6a2f65','Create Document',5,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','All_Create_Document'),('ECA884BB-CCB8-4723-A137-D73F988AE300','Add Document Index',1,'D6B63A2E-F7DB-4F83-B5DD-96A000D9A789','Add_Document_Index'),('ecf7dc42-fc44-4d1a-b314-d1ff71878d94','Delete Reminder',2,'97ff6eb0-39b3-4ddd-acf1-43205d5a9bb3','Delete_Reminder'),('ed9ec981-4e36-436d-962a-44c2f347213d','Edit Prompt Templates',5,'479b5173-369e-487a-817d-651d77782590','Edit_Prompt_Templates'),('f2bb13d3-c376-42ab-b804-952dfd8ce164','Add Workflow Settings',1,'309f72e8-6e93-43fa-9203-808b030a33f7','Add_Workflow_Settings'),('f54926e2-3ad3-40be-8f7e-14cab77e87bd','Edit Reminder',3,'97ff6eb0-39b3-4ddd-acf1-43205d5a9bb3','Edit_Reminder'),('f591c7be-4913-44f8-a74c-d2fc44dd5a3e','View Categories',5,'5a5f7cf8-21a6-434a-9330-db91b17d867c','View_Categories'),('f7dc4930-c87f-429d-be36-6b9817adf1e8','View Workflow Settings',4,'309f72e8-6e93-43fa-9203-808b030a33f7','View_Workflow_Settings'),('f8863c5a-4344-41cb-b1fa-83e223d6a7df','Download Document',8,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','All_Download_Document'),('faf1cb6f-9c20-4ca3-8222-32028b44e484','Send Email',15,'eddf9e8e-0c70-4cde-b5f9-117a879747d6','All_Send_Email'),('fb546f57-ce8a-4a58-8c9e-43955c3ea392','Get Document Summary',12,'fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned_Get_Document_Summary'),('FC1D752F-005B-4CAE-9303-B7557EEE7461','Manage Storage Settings',1,'FC1D752F-005B-4CAE-9303-B7557EEE7461','Manage_Storage_Settings'),('ff092131-a214-48c0-a8e3-68a8723840e1','Create User',3,'324bdc51-d71f-4f80-9f28-a30e8aae4009','Create_User'),('ffa7871f-de6f-4265-9548-4177e2bf5dfe','Share Folder',2,'da660d2d-450b-4825-9489-d87ef94ad6ff','Assigned_Share_Folder');
/*!40000 ALTER TABLE `PageActions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `PageHelpers`
--

DROP TABLE IF EXISTS `PageHelpers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `PageHelpers` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Code` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `PageHelpers`
--

LOCK TABLES `PageHelpers` WRITE;
/*!40000 ALTER TABLE `PageHelpers` DISABLE KEYS */;
INSERT INTO `PageHelpers` VALUES ('03beeb7c-5ebf-4722-8d9c-484ae8fd8500','AI_DOCUMENT_GENERATOR_LIST','AI Document Generator List – Manage Your Generated Documents','<p>The <strong>AI Document Generator List</strong> allows users to view, manage, and organize all documents created using the AI Document Generator. It provides a detailed table that makes it easy to track and work with your past document generations.</p><h3>&nbsp;<strong>Main Functionalities</strong></h3><ol><li><strong>View Generated Documents</strong></li></ol><p>All documents you’ve generated are listed in a simple, searchable table. Each entry includes:</p><ul><li><strong>Date &amp; Time</strong>: When the document was generated.</li><li><strong>Title</strong>: The name or type of the document (e.g., Business Proposal, Blog Post Idea).</li><li><strong>Prompt Input</strong>: A summary of what you asked the AI to generate.</li><li><strong>Selected Model</strong>: The AI model used (e.g., GPT-3.5 Turbo).</li></ul><p>You can click on a document row to <strong>view the full details</strong> of that document.</p><ol><li><strong>Search by Prompt Input</strong></li></ol><p>Use the built-in search bar to quickly find any document based on keywords from the prompt.<br>This is useful when you’ve generated many documents and want to locate a specific one without scrolling.</p><ol><li><strong>Delete a Document</strong></li></ol><p>If you no longer need a document, you can delete it directly from the list.</p><ul><li>Click the <strong>Delete</strong> icon or button next to the document entry.</li><li>A confirmation prompt will appear before deletion.</li><li>Once deleted, the document will be removed from your list permanently.</li></ul><h3>&nbsp;<strong>Why This Feature Is Useful</strong></h3><ul><li>Keeps your workspace organized.</li><li>Lets you revisit or review past work at any time.</li><li>Helps you avoid clutter by deleting unneeded documents.</li><li>Makes document management simple and efficient.</li></ul><h3><strong>Simple Workflow for Users</strong></h3><ol><li>Open the <strong>Document Generator List</strong>.</li><li>Use the <strong>search bar</strong> to filter by prompt keywords.</li><li>Click a row to <strong>view document details</strong>.</li><li>Click <strong>Delete</strong> if you want to remove a document.</li></ol>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('05f30717-e712-4bf8-be5d-8769328cba0f','AI_DOCUMENT_GENERATOR_DETAILS','AI Document Generator Details','<h3><strong>Overview of Your Document Creation Process</strong></h3><p>The <strong>AI Document Generator Details</strong> page helps you customize and generate documents based on the information you provide. Below is a breakdown of the steps and settings:</p><h3>1. <strong>Select AI Prompt Template</strong></h3><ul><li>Choose a <strong>pre-made template</strong> from options like Business Proposal, Blog Post, or Product Description.</li><li>Each template is designed to guide the AI in generating the type of document you need.</li></ul><h3>2. <strong>Prompt Input</strong></h3><ul><li><strong>Replace the placeholders</strong> in the template with your specific information.<ul><li>For example, if the template says:<br>“Write a detailed business proposal about <strong>product</strong>,”<br>you can replace <strong>product</strong> with “Electric Vehicles.”</li><li>This helps the AI understand exactly what you\'re asking for and generates the document accordingly.</li></ul></li></ul><h3>3. <strong>Language</strong></h3><ul><li>Select the <strong>language</strong> you want your document to be generated in. The default is <strong>English (USA)</strong>, but you can choose from other available languages as well.</li></ul><h3>4. <strong>Maximum Length</strong></h3><ul><li>Set a <strong>maximum word count</strong> for your document. This helps control how long or short the output will be.<ul><li>For example, if you set it to <strong>100 words</strong>, the AI will aim to generate a document that’s around that length.</li></ul></li></ul><h3>5. <strong>Creativity</strong></h3><ul><li>Choose how <strong>creative</strong> you want the document to be:<ul><li><strong>Formal</strong>: Ideal for professional documents.</li><li><strong>Creative</strong>: Great for informal or engaging content.</li><li><strong>Neutral</strong>: A balanced mix between formal and creative.</li></ul></li></ul><h3>6. <strong>Tone of Voice</strong></h3><ul><li>Select the <strong>tone</strong> you prefer for your document:<ul><li><strong>Professional</strong>: For business-like communication.</li><li><strong>Friendly</strong>: For casual, approachable writing.</li><li><strong>Persuasive</strong>: Ideal for marketing or proposal documents.</li></ul></li></ul><h3>7. <strong>Select AI Model</strong></h3><ul><li>Pick which <strong>AI model</strong> you want to use (e.g., <strong>GPT-3.5 Turbo</strong>). The model you choose can affect the document\'s style and quality.</li></ul><h3>8. <strong>Document Summary</strong></h3><ul><li>After filling out all the details, a <strong>summary of your document</strong> will be shown, which includes:<ul><li>The title or type of document.</li><li>The language and tone selected.</li><li>Any specific details based on your inputs.</li></ul></li></ul><p>This summary lets you review everything generated the document.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('0876e7ef-bf70-4a84-9ef4-ebd36f771390','FILE_UPLOAD_REQUEST','File Upload Request','<h2>File Upload Request</h2><p>The <strong>File Upload Request</strong> feature enables users to upload files securely via a shared link. Users can upload multiple files, provide additional details, and track the status of their submissions.</p><h2>Key Features:</h2><p><strong>1.View File Upload Request Details</strong>:<br>When accessing the upload link, users can see the following details:</p><ul><li><strong>Subject</strong>: The purpose or title of the file request.</li><li><strong>Allowed File Types</strong>: The types of files accepted (e.g., PDF, DOCX, JPG).</li><li><strong>Requested By</strong>: The name or email of the person requesting the files.</li><li><strong>Maximum File Size Upload</strong>: The largest file size allowed for each upload.</li></ul><p><strong>2.Upload Multiple Files</strong>:</p><ul><li>Users can upload multiple files by clicking the <strong>Add Another File</strong> button.</li><li>For each file:<ul><li>Select the file to upload.</li><li>Provide a <strong>File Name</strong> in the accompanying text box to describe the file.</li></ul></li></ul><p><strong>3.Uploaded Files List</strong>:<br>After uploading, users can view a list of their uploaded files, including:</p><ul><li><strong>File Name</strong>: The name of the uploaded file.</li><li><strong>Document Status</strong>: The status of the file (e.g., Pending, Approved, Rejected).</li><li><strong>Uploaded Date</strong>: The date and time the file was uploaded.</li></ul><p><strong>4.Track Document Status</strong>:</p><ul><li>Users can monitor the status of their submissions directly on the upload page:<ul><li><strong>Pending</strong>: Awaiting review by the requester.</li><li><strong>Approved</strong>: File has been accepted.</li><li><strong>Rejected</strong>: File has been rejected with a reason provided.</li></ul></li></ul><h2>How It Works:</h2><h3>1. Accessing the Upload Link:</h3><ul><li>Open the shared upload link provided in the request.</li><li>Review the file request details (e.g., subject, allowed file types, and maximum file size).</li></ul><h3>2. Uploading Files:</h3><ul><li>Click <strong>Add Another File</strong> to upload multiple files.</li><li>For each file:<ol><li>Click the <strong>Choose File</strong> button to select a file from your device.</li><li>Enter a descriptive <strong>File Name</strong> in the text box.</li></ol></li><li>Repeat the process to add additional files.</li><li>Once all files are selected, click <strong>Submit</strong> to upload.</li></ul><h3>3. Viewing Uploaded Files:</h3><ul><li>After submission, the uploaded files will appear in the <strong>Uploaded Files List</strong> below:<ul><li><strong>File Name</strong>: Displays the name entered for each file.</li><li><strong>Document Status</strong>: Shows the current status of the document (e.g., Pending, Approved, Rejected).</li><li><strong>Uploaded Date</strong>: Indicates when the file was submitted.</li></ul></li></ul><h3>4. Tracking and Resubmitting:</h3><ul><li>If a file is rejected, check the <strong>Document Status</strong> for a rejection reason.</li><li>Re-upload the corrected file by clicking <strong>Add Another File</strong> and submitting again.</li></ul><h2>Benefits:</h2><ul><li><strong>Ease of Use</strong>: Users can upload multiple files seamlessly with a clear interface.</li><li><strong>Transparency</strong>: Users can track the status of their submissions in real time.</li><li><strong>Flexibility</strong>: The ability to add descriptive names and upload multiple files ensures better organization.</li></ul><p>This feature makes it easy for users to submit files, manage uploads, and stay informed about the status of their submissions.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-09 06:35:12','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('0b6c1861-8bf3-413f-980c-1ea483cd5102','ARCHIVE_RETENTION_PERIOD','Archive Retention Period','<p><strong>What is it?</strong><br>Archive Retention Period allows you to automatically move documents to the <strong>delete</strong> after a selected number of days.</p><p><strong>Retention Options:</strong><br>You can choose to automatically delete documents after:</p><p>30 days</p><p>60 days</p><p>90 days</p><p>180 days</p><p>365 days</p><p><strong>How it works:</strong><br>Once this setting is enabled:</p><p>The system will monitor the age of each document.</p><p>When a document reaches the selected retention period (e.g., 30 days), it will be <strong>automatically deleted</strong>.</p><p><i>Enabling this feature helps keep your workspace organized by removing old documents automatically.</i></p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('0b8a7053-40e6-4919-94fe-30f66698a8be','DOCUMENT_SUMMARY','Get Document Summary using ChatGPT','<p>The <strong>\"Get Document Summary\"</strong> functionality allows users to upload a document and generate a concise summary using ChatGPT. \r\n    This feature helps users quickly understand the document\'s content without reading the entire text. \r\n    Below is a step-by-step guide on how this functionality works and how to configure it.</p>\r\n\r\n    <h2>How It Works</h2>\r\n    <ul>\r\n        <li><strong>Upload Document:</strong> The user selects and uploads a document (PDF, Word, or text file).</li>\r\n        <li><strong>Extract Text from Document:</strong> The system extracts the text content from the uploaded document.</li>\r\n        <li><strong>Send Text to ChatGPT API:</strong> The extracted text is sent to OpenAI\'s ChatGPT API for summarization.</li>\r\n        <li><strong>Receive Summary Response:</strong> ChatGPT processes the text and returns a summarized version.</li>\r\n        <li><strong>Display Summary to User:</strong> The generated summary is shown on the UI for easy reading.</li>\r\n    </ul>\r\n','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('0cc83192-f05b-4c97-ab20-f7f3b5ba16d0','REMINDERS','Reminders','<p>The <strong>\"Reminders\"</strong> page is the central hub for managing reminders within CMR DMS, where users can create, view, and manage reminders or notifications related to documents or other activities. Reminders can be set to repeat at regular intervals and can be associated with a specific document for efficient tracking of tasks and activities.</p><h3>Main components:</h3><ol><li><strong>\"Add Reminder\" Button</strong>:<ul><li>Allows users to create a new reminder.</li><li>Upon clicking, it opens a form where details such as subject, message, frequency, associated document, and reminder date can be entered.</li></ul></li><li><strong>Reminders Table</strong>:<ul><li>Displays all created reminders in a tabular format.</li><li>Each entry includes:<ul><li>Start date</li><li>End date</li><li>Reminder subject</li><li>Associated message</li><li>Recurrence frequency</li><li>Associated document (if applicable)</li></ul></li></ul></li></ol><h3>\"Add Reminder\" Form:</h3><p>When users click on the <strong>\"Add Reminder\"</strong> button, a form opens with the following fields:</p><ul><li><strong>Subject</strong>: The title or topic of the reminder (e.g., \"Document Review\").</li><li><strong>Message</strong>: Additional details about the reminder (e.g., \"Review the document by X date\").</li><li><strong>Repeat Reminder</strong>: Sets the recurrence frequency, with options such as:<ul><li>Daily</li><li>Weekly</li><li>Monthly</li><li>Semi-annually</li></ul></li><li><strong>Send Email</strong>: An option to send an email notification when the reminder is activated.</li><li><strong>Select Users</strong>: Allows selecting users to whom the reminder will be sent. It can be customized for specific teams or individuals.</li><li><strong>Reminder Date</strong>: The date and time when the reminder will be activated and sent.</li></ul><h3>How to add a new reminder:</h3><ol><li>Navigate to the <strong>\"Reminders\"</strong> page.</li><li>Click the <strong>\"Add Reminder\"</strong> button.</li><li>Fill in the form fields with the necessary information.</li><li>After entering all the details, click <strong>\"Save\"</strong> or <strong>\"Add\"</strong> to save the reminder in the system.</li></ol><h3>\"Add Reminder\" Functionality in the \"Manage Reminders\" section:</h3><p>This is the dedicated place for creating and managing notifications related to events or tasks. The <strong>\"Add Reminder\"</strong> functionality offers full customization, and reminders can be sent to selected users.</p><ul><li><strong>Subject</strong>: Enter a descriptive title for the reminder.</li><li><strong>Message</strong>: Add a clear and concise message to detail the purpose of the reminder.</li><li><strong>Repeat Reminder</strong>: Set whether the reminder will be repeated periodically (daily, weekly, etc.).</li><li><strong>Send Email</strong>: If this option is checked, the reminder will also be sent as an email.</li><li><strong>Select Users</strong>: Select users from the system\'s list to whom the reminder will be sent.</li><li><strong>Reminder Date</strong>: Set the date and time for the reminder to be triggered.</li></ul>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 04:41:27','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('0d0a1c72-4e06-4793-923a-3fe1fe1695d5','ARCHIVE_DOCUMENTS','Archive Documents','<h2>Archive Documents</h2><p>The <strong>Archive Documents</strong> feature provides a centralized location to manage, share, and restore archived files. It ensures secure storage and easy access to older documents while offering several actions to organize and use these files effectively.</p><h2>Key Features:</h2><p><strong>1.Get Shareable Link</strong>:</p><ul><li>Generate a unique, shareable link for any archived document.</li><li>Use this link to securely share the document with others.</li></ul><p><strong>2.View Document</strong>:</p><ul><li>Preview the archived document directly without downloading.</li><li>Supports common file types like PDF, DOCX, JPG, and more.</li></ul><p><strong>3.Restore Document</strong>:</p><ul><li>Restore an archived document to its original location or active status.</li><li>This allows you to reinstate files that may be needed again.</li></ul><p><strong>4.Download Document</strong>:</p><ul><li>Download a copy of the archived document to your device.</li><li>Ideal for offline access or sharing outside the system.</li></ul><p><strong>5.Version History</strong>:</p><ul><li>Access the complete version history of the document.</li><li>View changes, updates, or previous versions and restore a specific version if needed.</li></ul><p><strong>6.Delete Document</strong>:</p><ul><li>Permanently remove the archived document from the system.</li><li>Ensure careful use as deleted files cannot be recovered.</li></ul><p><strong>7.Send via Email</strong>:</p><ul><li>Email the archived document directly to one or more recipients.</li><li>Include a personalized message if required.</li></ul><p><strong>8.Filter Data</strong>:</p><ul><li>Quickly find specific documents using advanced filters, such as:<ul><li><strong>Document Name</strong></li><li><strong>Uploaded Date</strong></li><li><strong>Status</strong></li><li><strong>etc.</strong></li></ul></li></ul><h2>How It Works:</h2><h3>1. Accessing Archived Documents:</h3><ul><li>Navigate to the <strong>Archive Documents</strong> page.</li><li>Use the search bar or filters to locate the desired document.</li></ul><h3>2. Managing Documents:</h3><ul><li><strong>Get Shareable Link</strong>:<ul><li>Click on the \"Share\" icon next to the document.</li><li>Copy the generated link and share it securely.</li></ul></li><li><strong>View Document</strong>:<ul><li>Click on the \"View\" option to preview the document.</li></ul></li><li><strong>Restore Document</strong>:<ul><li>Select the document and click \"Restore\" to move it back to its active location.</li></ul></li><li><strong>Download Document</strong>:<ul><li>Click the \"Download\" button to save a copy of the file to your device.</li></ul></li><li><strong>Version History</strong>:<ul><li>Open the document’s version history to review or restore previous versions.</li></ul></li><li><strong>Delete Document</strong>:<ul><li>Click the \"Delete\" button to permanently remove the file. Confirm the action if prompted.</li></ul></li><li><strong>Send via Email</strong>:<ul><li>Select the \"Send Email\" option, enter the recipient’s email address, and send the document with an optional message.</li></ul></li></ul><h3>3. Using Filters:</h3><ul><li>Apply filters like <strong>Document Name</strong>, <strong>Uploaded Date</strong>, or <strong>File Type</strong> to refine the list of archived documents.</li><li>Combine multiple filters for more specific results.</li></ul><h2>Benefits:</h2><ul><li><strong>Efficient Organization</strong>: Keep archived files easily accessible and manageable.</li><li><strong>Flexible Sharing</strong>: Share documents securely with links or via email.</li><li><strong>Version Control</strong>: Maintain a history of document changes for accountability.</li><li><strong>Easy Recovery</strong>: Restore archived files when needed without losing data.</li></ul><p>This functionality ensures a smooth workflow for managing archived documents while maintaining security and flexibility.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-09 10:49:21','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('0fae65e2-091d-469b-8a2a-9bb363ba8290','DOCUMENTS_AUDIT_TRAIL','Document audit history','<p><strong>General Description:</strong></p><p>The \"Document Audit History\" page provides a detailed view of all actions performed on documents within the DMS. It allows administrators and users with appropriate permissions to monitor and review document-related activities, ensuring transparency and information security.</p><p><strong>Main Components:</strong></p><p><strong>Search Boxes:</strong></p><ul><li><strong>By Document Name:</strong> Allows users to search for a specific document by entering its name or other details.</li><li><strong>By Meta Tag:</strong> Users can enter meta tags to filter and search for specific document-related activities.</li><li><strong>By User:</strong> Enables filtering activities based on the user who performed the operation.</li></ul><p><strong>List of Audited Documents:</strong></p><p>Displays all actions taken on documents in a tabular format.</p><p>Each entry includes details of the action, such as the date, document name, Category, operation performed, who performed the operation, to which user, and to which role the operation was directed.</p><p>Users can click on an entry to view additional details or access the associated document.</p><p><strong>List Sorting:</strong></p><p>Users can sort the list by any of the available columns, such as \"Date,\" \"Name,\" \"Category Name,\" \"Operation,\" \"Performed by,\" \"Directed to User,\" and \"Directed to Role.\"</p><p>This feature makes it easier to organize and analyze information based on specific criteria.</p><p><strong>How to Search the Audit History:</strong></p><ul><li>Enter your search criteria in the corresponding search box (document name, meta tag, or user).</li><li>The search results will be displayed in the audited documents list.</li></ul><p><strong>How to Sort the List:</strong></p><ul><li>Click on the column title by which you want to sort the list (e.g., \"Date\" or \"Name\").</li><li>The list will automatically reorder based on the selected criterion.</li></ul>','2023-06-03 05:17:16','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 05:50:16','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('1232317d-0d32-43bd-97b9-14c5963fd309','ARCHIVE_FOLDERS','Archive Folders','<p>The <strong>Archive Folders</strong> feature provides a centralized location to manage, share, and restore archived folders. It ensures secure storage and easy access to older folders while offering several actions to organize and use these folders effectively.</p><h3><strong>Key Features:</strong></h3><p><strong>Restore Folder:</strong></p><ul><li>Restore an archived folder to its original location or active status.</li><li>All documents inside the folder will also be restored.</li></ul><p><strong>Delete Folder:</strong></p><ul><li>Permanently remove the archived folder from the system.</li><li>Ensure careful use as deleted folders cannot be recovered.</li><li>&nbsp;</li><li><h3><strong>How It Works:</strong></h3></li><li><h4><strong>1. Accessing Archived Folders:</strong></h4></li><li>Navigate to the <strong>Archive Folders</strong> page.</li><li>Use the search bar or filters to locate the desired folder.</li><li><h4><strong>2. Managing Folders:</strong></h4></li><li>✅ <strong>Restore Folder:</strong></li><li>Select the folder and click <strong>\"Restore\"</strong> to move it back to its active location.</li><li>All documents inside the folder will also be reinstated.</li><li>✅ <strong>Delete Folder:</strong></li><li>Click the \"Delete\" button to permanently remove the folder. Confirm the action if prompted.</li><li><h4><strong>3. Using Filters:</strong></h4></li><li>Apply filters like <strong>Folder Name, Created Date, or Owner</strong> to refine the list of archived folders.</li><li>Combine multiple filters for more specific results.</li><li><h3><strong>Benefits:</strong></h3></li><li>✔ <strong>Efficient Organization:</strong> Keep archived folders easily accessible and manageable.<br>✔ <strong>Flexible Sharing:</strong> Share folders securely with links or via email.<br>✔ <strong>Version Control:</strong> Maintain a history of folder changes for accountability.<br>✔ <strong>Easy Recovery:</strong> Restore archived folders when needed without losing data.</li><li>This functionality ensures a smooth workflow for managing archived folders while maintaining security and flexibility.</li></ul>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('23b67895-da17-44e6-8546-75f39f25a02e','ASSIGN_DOCUMENTS_FOLDER_VIEW','Documents Folder View','<p>The \"Documents - Folder View\" is designed to provide users with an organized and efficient way to manage document folders assigned to them. In addition to managing individual documents, users can now share entire folders with other users or roles, making collaboration seamless.</p><h3><strong>Add Folder Button</strong></h3><ul><li>Users can create new folders to better categorize and manage their assigned documents.</li><li>Clicking the \"Add Folder\" button opens a form or pop-up where users can specify the folder name and optional details.</li><li>Users can then upload or move documents into the newly created folder.</li></ul><h2><strong>Folder Management &amp; Actions</strong></h2><p>Each folder in the \"Assigned Documents - Folder View\" is listed with relevant details, such as:</p><p><strong>Folder Name</strong></p><p><strong>Doc Number</strong></p><p><strong>Created By</strong></p><p><strong>Creation Date</strong></p><p><strong>Action</strong></p><h3><strong>Available Actions for Each Folder</strong></h3><h4><strong>Share Folder</strong></h4><ul><li>Users can share an entire folder with other users or roles, similar to document sharing.</li><li>Permissions can be set for each recipient, such as:<ul><li><strong>View Only:</strong> Users can view documents but not edit or download them.</li><li><strong>Edit:</strong> Users can modify documents within the shared folder.</li><li><strong>Upload New Versions:</strong> Users can upload updated versions of documents.</li><li><strong>Full Access:</strong> Users can manage the folder and its documents entirely.</li></ul></li><li>Shared folders appear in the recipient’s assigned folder list, with access restrictions based on permissions.</li></ul><h2><strong>How to Share a Folder</strong></h2><ol><li>Click the <strong>\"Share\"</strong> option next to the folder.</li><li>A pop-up window will appear to enter user names, roles, or email addresses.</li><li>Set specific permissions for each user or role.</li><li>Click <strong>\"Share\"</strong> to finalize the action.</li><li>The recipients will receive a notification and gain access to the shared folder based on the assigned permissions.</li><li>Shared folders function similarly to shared documents, with users having controlled access based on the granted permissions.</li></ol><h2><strong>Folder View - Document Actions</strong></h2><p>Inside each folder, users can view and manage assigned documents. Actions available for documents inside folders include:</p><ul><li><strong>Edit</strong>: Modify document details.</li><li><strong>Share</strong>: Share the document individually with users or roles.</li><li><strong>View</strong>: Open the document in a viewer.</li><li><strong>Upload a Version</strong>: Add an updated version of the document.</li><li><strong>Version History</strong>: View and manage previous versions.</li><li><strong>Comment</strong>: Add or view discussions on the document.</li><li><strong>Add Reminder</strong>: Set a reminder for an event related to the document.</li></ul><p>&nbsp;</p><p>The <strong>Documents - Folder View</strong> enhances document management by introducing folder-based organization and streamlined sharing capabilities, ensuring a more efficient workflow for users.</p><p>&nbsp;</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('25ccccd4-bd60-4f8b-8bc1-c49eca98fb49','EMAIL_SMTP_SETTINGS','SMTP Email Settings','<p>The <strong>\"Email SMTP Settings\"</strong> page within CMR DMS allows administrators to configure and manage the SMTP settings for sending emails. This ensures that emails sent from the system are correctly and efficiently delivered to recipients.</p><p><strong>Key Components:</strong></p><ul><li><p><strong>SMTP Settings Table:</strong> Displays all configured SMTP settings in a tabular format.</p><p>Each entry in the table includes details such as the username, host, port, and whether that configuration is set as the default.</p></li><li><p><strong>\"Add Settings\" Button:</strong> Allows administrators to add a new SMTP configuration.</p><p>Clicking the button opens a form where details like username, host, port, and the option to set it as the default configuration can be entered.</p></li></ul><p><strong>\"Add Settings\" Form:</strong></p><p>This form opens when administrators click the \"Add Settings\" button and includes the following fields:</p><ul><li><strong>Username:</strong> The username required for authentication on the SMTP server.</li><li><strong>Host:</strong> The SMTP server address.</li><li><strong>Port:</strong> The port on which the SMTP server listens.</li><li><strong>Is Default:</strong> A checkbox that allows setting this configuration as the default for sending emails.</li></ul><p><strong>How to Add a New SMTP Configuration:</strong></p><ol><li>Click the \"Add Settings\" button.</li><li>The \"Add Settings\" form will open, where you can enter the SMTP configuration details.</li><li>Fill in the necessary fields and select the desired options.</li><li>After completing the details, click \"Save\" or \"Add\" to add the configuration to the system.</li></ol>','2023-06-03 05:25:45','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-04 14:30:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('2b728c10-c0b3-451e-8d08-2be1e3f6d5b3','USERS','Users','<p><strong>The \"Users\" page is the central hub for managing all registered users in CMR DMS. Here, administrators can add, edit, or delete users, as well as manage permissions and reset passwords. Each user has associated details such as first name, last name, mobile phone number, and email address.</strong></p><p><strong>Main Components:</strong></p><ul><li><p><strong>\"Add User\" Button:</strong> Allows administrators to create a new user in the system.</p><p>Opens a form where details such as first name, last name, mobile phone number, email address, password, and password confirmation can be entered.</p></li><li><p><strong>List of Existing Users:</strong> Displays all registered users in the system in a tabular format.</p><p>Each entry includes the user’s email address, first name, last name, and mobile phone number.</p><p>Next to each user, there is an action menu represented by three vertical dots.</p></li><li><p><strong>Action Menu for Each User:</strong> This menu opens by clicking on the three vertical dots next to each user.</p><p>Includes the options:</p><ul><li><strong>Edit:</strong> Allows modification of the user’s details.</li><li><strong>Delete:</strong> Removes the user from the system. This action may require confirmation to prevent accidental deletions.</li><li><strong>Permissions:</strong> Opens a window or form where administrators can set or modify the user’s permissions.</li><li><strong>Reset Password:</strong> Allows administrators to initiate a password reset process for the selected user.</li></ul></li></ul><p><strong>How to Add a New User:</strong></p><ol><li>Click on the \"Add User\" button.</li><li>A form will open where you can enter the user’s details: first name, last name, mobile phone number, email address, password, and password confirmation.</li><li>After completing the details, click \"Save\" or \"Add\" to add the user to the system.</li></ol>','2023-06-03 05:21:00','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-04 14:17:57','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('2dd28c72-3ed4-4f75-b23b-63cadcaa3982','ALL_DOCUMENTS','All Documents','<p>The <strong>\"All Documents\"</strong> page provides a complete overview of all documents uploaded in the DMS. It is the ideal place to search, view, manage, and distribute all available documents in the system.</p><p><strong>Main Components:</strong></p><ul><li><strong>\"Add Document\" Button:</strong> Allows any user with appropriate permissions to upload a new document into the system.<ul><li>Opens a form or a pop-up window where files can be selected and uploaded.</li></ul></li><li><strong>Search Box (by name):</strong> Allows users to search for a specific document by entering its name or other details.</li><li><strong>Search Box (by meta tags):</strong> Users can enter Meta tags to filter and search for specific documents.</li><li><strong>Category Dropdown:</strong> A dropdown menu that allows users to filter documents by Category.</li><li><strong>Status Dropdown:</strong>  There is an option users to store A dropdown menu that allows users to filter documents by Status.</li><li><strong>Storage Dropdown: </strong>The application lets users store documents in various storage options, such as AWS S3, Cloudflare R2, and local storage. Users can easily search for documents by selecting the desired storage option from a dropdown menu.</li><li><strong>Search Box (by creation date):</strong> Allows users to search for documents based on their creation date.</li><li><strong>List of All Uploaded Files:</strong> Displays all documents available in the system.<ul><li>Each entry includes document details such as name, creation date, Category, status and storage.</li></ul></li><li><strong>Document Actions Menu:</strong> Alongside each document in the list, users will find an actions menu allowing them to perform various operations on the document:<ul><li><strong>Edit:</strong> Modify the document details, such as its name or description.</li><li><strong>Share:</strong> Share the document with other users or roles within the system.</li><li><strong>Get Shareable Link:</strong> Users can generate a shareable link to allow anonymous users to access documents. They can also protect the link with a password and set an expiration period, ensuring the link remains active only for the selected duration. Additionally, the link includes an option for recipients to download the shared document.</li><li><strong>View:</strong> Open the document for viewing.</li><li><strong>Upload a New Version:</strong> Add a new version of the document.</li><li><strong>Version History:</strong> Users can view all previous versions of a document, with the ability to restore any earlier version as needed. Each version can also be downloaded for offline access or review.</li><li><strong>Comment:</strong> Add or view comments on the document.</li><li><strong>Add Reminder:</strong> Set a reminder for the document.</li><li><strong>Send as Email:</strong> Send the document as an attachment via email.</li><li><strong>Delete:</strong> Remove the document from the system.</li></ul></li></ul><p><strong>Document Sharing:</strong></p><p>Users can select one, multiple, or all documents from the list and use the sharing option to distribute the selected documents to other users. This feature facilitates the mass distribution of documents to specific users or groups.</p>','2023-06-03 05:12:00','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 06:52:18','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('3146815f-2d9d-4b9c-963b-ab2dcfa6aa1d','REQUEST_DOCUMENT_THROUGH_WORKFLOW','Request Document Through Workflow','<h3>Request Document through Workflow</h3><p>Requesting a document through a workflow ensures a systematic and organized process. Follow these steps for a smooth experience:</p><p><strong>1.Select Workflow</strong><br>Begin by choosing the appropriate workflow. A workflow is a predefined process designed to handle document requests efficiently.</p><p><strong>2.Request Document Category</strong><br>Specify the category of the document you are requesting. Categories help in organizing and identifying the purpose of the requested document.</p><p><strong>3.Requested Document Name</strong><br>Clearly mention the name of the document you are requesting. This ensures there is no confusion about the specific document required.</p><p><strong>4.Instruction to Upload Document</strong><br>Provide clear and detailed instructions for uploading the document. Include any file format, size limitations, or additional details to make the process seamless.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-10 05:08:33','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('32688863-5177-4747-8e3d-64edce88c577','MANAGE_CLIENT','Manage Client','<p>The <strong>Manage Client</strong> feature makes it easy to add new clients or edit existing client details. Here’s how you can use it:</p><h4><strong>Add New Client</strong></h4><p>1.Click the <strong>\"Add Client\"</strong> button.</p><p>2.A form will appear where you can enter the following details:</p><p><strong>Company/Person Name</strong>: Enter the name of the company or individual client.</p><p><strong>Contact Person</strong>: Provide the name of the main contact person.</p><p><strong>Email</strong>: Enter the client’s email address.</p><p><strong>Mobile Number</strong>: Add the client’s mobile number for quick contact.</p><p>3.Once all the details are filled in, click the <strong>\"Save\"</strong> button to add the new client to the list.</p><p>4.The newly added client will now appear in the <strong>Clients List</strong>.</p><h4><strong>Edit Existing Client</strong></h4><p>1.In the <strong>Clients List</strong>, locate the client whose details you want to edit.</p><p>2.Click the <strong>Edit</strong> button in the <strong>Action</strong> column.</p><p>3.A form will open, pre-filled with the client’s existing details.</p><p>4.Update any necessary fields, such as:</p><p>Correcting the email address or phone number.</p><p>Changing the contact person or company name.</p><p>5.After making the changes, click the <strong>\"Save\"</strong> button to update the client’s information.</p><p>6.The changes will reflect immediately in the <strong>Clients List</strong>.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-10 05:08:33','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('37ba35ed-e90a-42d5-85a4-d57b86ba684c','ADD_PROMPT_TEMPLATE','Add Prompt Template','<h3> <strong>Add Prompt Template – Explanation</strong></h3><p>This section allows users to <strong>create reusable AI prompt templates</strong>. These templates tell the AI how to generate specific types of content (like product descriptions, emails, summaries, etc.), and can include <strong>dynamic placeholders</strong> that will be replaced with user input later.</p><h3> <strong>Field Explanations</strong></h3><h4> <strong>Name</strong> <i>(Required)</i></h4><p>Enter a short title for your template.</p><blockquote><p>Example: <strong>\"Product Description Generator\"</strong></p></blockquote><h4> <strong>Description</strong> <i>(Optional)</i></h4><p>Briefly explain what this template does and when to use it.</p><blockquote><p>Example: <i>\"Generates a marketing-friendly product description based on product name and features.\"</i></p></blockquote><h4> <strong>Prompt Input</strong> <i>(Required)</i></h4><p>This is where you write the prompt the AI will follow.<br>Use <strong>dynamic words</strong> inside double asterisks to mark where the AI should insert user-specific content.</p><blockquote><p> Format: **DynamicWord**<br>Example:<br><i>\"Write a professional description for the product <strong>ProductName</strong> that highlights its key benefits: <strong>ProductBenefits</strong>.\"</i></p></blockquote><p>These dynamic words will be shown as input fields when the user runs the template.</p><h3> <strong>Why Use Dynamic Words?</strong></h3><p>Dynamic words make the template flexible. You can reuse the same prompt for different inputs, and the system will ask the user to fill in each **DynamicWord**.</p><ul><li>It’s like a fill-in-the-blanks AI form.</li><li>Helps non-technical users interact with AI easily.</li></ul>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('39ca4a0c-97a7-44a4-9f99-e98ed7c0dad7','MANAGE_ALLOW_FILE_EXTENSION','Manage Allow File Extension','<p><strong>Manage Allowed File Extensions</strong></p><p>This functionality allows users to control which file types are permitted for upload in the application. Users can easily configure allowed file extensions by selecting the desired file types and specifying their extensions in a provided configuration interface. Here\'s how it works:</p><ol><li><strong>Select File Types</strong>: Users can choose from a predefined list of file types (e.g., images, documents, videos) or manually add custom types.</li><li><strong>Add Extensions</strong>: For each file type, users can specify the associated file extensions (e.g., .jpg, .pdf, .mp4).</li><li><strong>Apply Changes</strong>: Once configured, the application will enforce these rules, ensuring only the specified file types can be uploaded.</li><li><strong>Easy Management</strong>: Users can modify, add, or remove allowed extensions anytime, making the system flexible and easy to update.</li></ol><p>This functionality simplifies file type management and ensures compliance with application requirements or security policies.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-24 07:14:20','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('3e0fe36d-cde5-4bd9-b65d-cfaeadcffce3','COMPANY_PROFILE','Company Profile','<p>Here’s a detailed description of the functionality for managing the company profile, focusing on the company name, lo;, and banner lo; on the login screen.</p><h3> </h3><h4>Overview</h4><p>The Company Profile feature allows users to customize the branding of the application by entering the company name and uploading lo;s. This customization will reflect on the login screen, enhancing the professional appearance and brand identity of the application.</p><h4>Features</h4><ol><li><h4><strong>Company Name</strong></h4><ul><li><strong>Input Field:</strong><ul><li>Users can enter the name of the company in a text input field.</li><li><strong>Validation:</strong><ul><li>The field will have validation to ensure the name is not empty and meets any specified length requirements (e.g., minimum 2 characters, maximum 50 characters).</li><li><strong>Browser Title Setting:</strong></li><li>Upon saving the company name, the application will dynamically set the browser title to match the company name, improving brand visibility in browser tabs.</li></ul></li></ul></li></ul></li><li><h4><strong>Lo; Upload</strong></h4><ul><li><strong>Upload Button:</strong><ul><li>Users can upload a company lo; that will be displayed in the header of the login page.</li><li><strong>File Requirements:</strong><ul><li>Supported file formats: PNG, JPG, JPEG (with size limits, e.g., up to 2 MB).</li><li>Recommended dimensions for optimal display (e.g., width: 200px, height: 100px).</li></ul></li></ul></li><li><strong>Preview:</strong><ul><li>After uploading, a preview of the lo; will be displayed to confirm the upload.</li></ul></li></ul></li><li><h4><strong>Banner Lo; Upload</strong></h4><ul><li><strong>Upload Button:</strong><ul><li>Users can upload a banner lo; that will appear prominently on the login screen.</li><li><strong>File Requirements:</strong><ul><li>Supported file formats: PNG, JPG, JPEG (with size limits, e.g., up to 3 MB).</li><li>Recommended dimensions for optimal display (e.g., width: 1200px, height: 300px).</li></ul></li></ul></li><li><strong>Preview:</strong><ul><li>A preview of the banner lo; will be displayed after the upload for confirmation.</li></ul></li></ul></li><li><h4><strong>User Interaction Flow</strong></h4><ul><li><h4><strong>Navigating to the Company Profile:</strong></h4><ul><li>Users can access the company profile settings from the application’s settings menu or administration panel.</li></ul></li><li><strong>Editing Company Profile:</strong><ul><li>Users enter the company name, upload the lo;, and the banner lo;.</li><li>A \"Save Changes\" button will be available to apply the modifications.</li></ul></li><li><strong>Saving Changes:</strong><ul><li>Upon clicking \"Save Changes,\" the uploaded lo;s and company name will be saved and reflected on the login screen.</li><li>Confirmation messages will be displayed to indicate successful updates.</li></ul></li></ul></li><li><strong>Display on Login Screen</strong><ul><li><strong>Header Display:</strong><ul><li>The company lo; will be displayed in the header at the top of the login page, maintaining a consistent branding experience.</li></ul></li><li><strong>Banner Display:</strong><ul><li>The banner lo; will be displayed prominently below the header, enhancing the visual appeal of the login interface.</li></ul></li></ul></li></ol><h3>Summary</h3><p>The Company Profile functionality allows for a customizable branding experience, enabling users to set their company name and lo;s that will be visible on the login screen. This feature enhances user engagement and presents a professional image right from the login phase of the application.</p>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 08:59:50','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('44d08495-12ef-40bd-b777-a94a6c784035','FILE_REQUEST_UPLOADED_DOCUMENTS','File Request Uploaded Documents','<h2>File Request Uploaded Documents</h2><p>The <strong>File Request Uploaded Documents</strong> feature allows you to manage the documents submitted through your file request link. You can review, approve, or reject uploaded files and provide feedback or reasons for rejection.</p><h2>Key Features:</h2><p><strong>1.View Uploaded Documents</strong>:</p><ul><li>Access all documents submitted via the file request link.</li><li>See details such as:<ul><li>File Name</li><li>Upload Date</li><li>Document Status</li><li>Reason</li></ul></li></ul><p><strong>2.Approve Documents</strong>:</p><ul><li>Mark documents as <strong>Approved</strong> if they meet your requirements.</li><li>Approved documents will be saved and marked as finalized.</li></ul><p><strong>3.Reject Documents</strong>:</p><ul><li>Reject documents that do not meet the criteria or need corrections.</li><li>When rejecting a document:<ul><li>Add a <strong>Comment</strong> to explain the reason for rejection.</li><li>This ensures users understand what needs to be corrected or resubmitted.</li></ul></li></ul><p><strong>4.Document Preview</strong>:</p><ul><li>View uploaded documents directly before approving or rejecting them.</li><li>Supports previewing common file types such as PDF, DOCX, JPG, and PNG.</li></ul><p><strong>5.Status Tracking</strong>:</p><ul><li>Each document will have a status indicator:<ul><li><strong>Pending</strong>: Awaiting review.</li><li><strong>Approved</strong>: Accepted and finalized.</li><li><strong>Rejected</strong>: Requires resubmission with a reason provided.</li></ul></li></ul><h2>How It Works:</h2><h3>1. Viewing Uploaded Documents:</h3><ul><li>Go to the <strong>File Request Uploaded Documents</strong> page.</li><li>Select the relevant file request from the list.</li><li>All submitted documents for that request will be displayed.</li></ul><h3>2. Approving Documents:</h3><ul><li>Click on the document you want to approve.</li><li>Review the document using the preview feature.</li><li>If the document meets your requirements, click <strong>Approve</strong>.</li><li>The status will change to <strong>Approved</strong>.</li></ul><h3>3. Rejecting Documents:</h3><ul><li>Click on the document you want to reject.</li><li>Use the preview feature to review the document.</li><li>If the document does not meet the requirements:<ul><li>Click <strong>Reject</strong>.</li><li>Enter a <strong>Reason for Rejection</strong> in the comment box (e.g., \"Incorrect file format\" or \"Incomplete information\").</li><li>Save the rejection and notify the user to resubmit.</li></ul></li></ul><h3>4. Adding Comments for Rejected Documents:</h3><ul><li>When rejecting a document, always provide a clear and actionable comment.</li><li>Examples of comments:<ul><li>\"Please upload a file in PDF format.\"</li><li>\"The document is missing required signatures.\"</li><li>\"File size exceeds the maximum limit; please compress and reupload.\"</li></ul></li></ul><h2>Benefits:</h2><ul><li><strong>Efficient Review</strong>: Quickly review and take action on uploaded documents.</li><li><strong>Clear Communication</strong>: Provide feedback for rejected documents, ensuring users know what to fix.</li><li><strong>Organized Workflow</strong>: Keep track of document statuses with easy-to-use status indicators.</li></ul><p>This feature ensures a smooth and transparent document review process for both you and the users.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-09 06:29:06','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('4654e4de-f167-4329-8a43-cd0e56f430e1','CURRENT_WORKFLOWS','Current Workflows','<ul><li><h3>Current Workflow Page Overview</h3></li><li>The <strong>Current Workflow Page</strong> provides users with a personalized view of workflows they have rights to manage or view. This page displays only the workflows associated with the user, ensuring they can easily track and manage their tasks.</li><li><h4><strong>Key Features</strong></h4></li><li><strong>User-Specific Workflow Display</strong>:<ul><li>This page shows <strong>only the workflows</strong> that the user has permission to access and manage.</li><li>The workflows are categorized based on their statuses:<ul><li><strong>Completed</strong>: Workflows that the user has finished or completed steps for.</li><li><strong>Initiated</strong>: Workflows the user has started but are awaiting further progress.</li><li><strong>In Progress</strong>: Workflows where the user is actively involved in ongoing steps.</li><li><strong>Cancelled</strong>: Workflows the user has been part of that were cancelled before completion.</li></ul></li></ul></li><li><strong>Workflow Details in Graphical View</strong>:<ul><li>The workflows are represented graphically to show:<ul><li>The flow of steps and transitions.</li><li><strong>Completed Transitions</strong>: Clearly marked for easy recognition.</li><li><strong>Pending Transitions</strong>: Distinctly highlighted to indicate remaining tasks.</li></ul></li></ul></li><li><strong>Workflow Information Table</strong>:<ul><li>For each workflow, users can view detailed information, including:<ul><li><strong>Workflow Name</strong>: Unique name of the workflow.</li><li><strong>Workflow Status</strong>: Current status (Completed, Initiated, In Progress, Cancelled).</li><li><strong>Initiated By</strong>: The user who initiated the workflow.</li><li><strong>Document Name</strong>: Associated document, if applicable.</li><li><strong>Workflow Step</strong>: The current step(s) the user is involved in.</li><li><strong>Workflow Step Status</strong>: Status of each step (Completed, Pending).</li><li><strong>Performed By</strong>: User(s) responsible for the steps.</li><li><strong>Transition Status</strong>: Whether transitions are completed or pending.</li></ul></li></ul></li><li><strong>Interactive Details</strong>:<ul><li>Users can click on any step or transition to access:<ul><li>Detailed information about that step/transition.</li><li>History and status of the action.</li><li>Relevant timestamps and actions taken.</li></ul></li></ul></li><li><h3>Benefits</h3></li><li>The <strong>Current Workflow Page</strong> is designed for users to have a focused, user-specific view of workflows they have rights to manage. This ensures:</li><li><strong>Personalized Workflow Management</strong>: Only workflows the user is authorized to access are shown.</li><li><strong>Efficient Tracking</strong>: Users can easily track progress of workflows they’re involved in.</li><li><strong>Clear Visibility</strong>: Understanding of the workflow status, transitions, and who is performing each step.</li><li>This page provides a secure and streamlined experience for users to manage their assigned workflows effectively.</li></ul>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-20 06:11:48','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('509dfdb8-8e5c-4370-8427-f6a9c2c78007','ROLE_USER','Role users','<p><strong>The \"User with Role\" page is dedicated to assigning specific roles to users within the DMS. It allows administrators to associate users with particular roles using an intuitive \"drag and drop\" system. Users can be moved from the general user list to the \"Users with Role\" list, thereby assigning them the selected role.</strong></p><h3><strong>Main Components:</strong></h3><ul><li><strong>Title \"User with Role\":</strong> Indicates the purpose and functionality of the page.</li><li><strong>Department:</strong> Displays the currently selected department, in this case, \"Approvals.\"<ul><li>There may be an option to change the department if needed.</li></ul></li><li><strong>Select Role:</strong> A dropdown menu or selection box where administrators can choose the role they wish to assign to users.<ul><li>Once a role is selected, users can be moved into the \"Users with Role\" list to assign them that role.</li></ul></li><li><strong>Note:</strong> A short instruction explaining how to use the page, indicating that users can be moved from the \"All Users\" list to the \"Users with Role\" list to assign them a role.</li><li><strong>\"All Users\" and \"Users with Role\" Lists:</strong><ul><li><strong>\"All Users\":</strong> Displays a complete list of all registered users in the CMR DMS.</li><li><strong>\"Users with Role\":</strong> Displays the users who have been assigned the selected role.</li><li>Users can be moved between these lists using the \"drag and drop\" functionality.</li></ul></li></ul><h3><strong>How to Assign a Role to a User:</strong></h3><ol><li>Select the desired role from the \"Select Role\" box.</li><li>Locate the desired user in the \"All Users\" list.</li><li>Using the mouse or a touch device, drag the user from the \"All Users\" list and drop them into the \"Users with Role\" list.</li><li>The selected user will now be associated with the chosen role and will appear in the \"Users with Role\" list.</li></ol>','2023-06-03 05:23:23','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 06:17:45','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('5d15d912-674b-47af-ade8-35013e4c95c4','DOCUMENT_COMMENTS','Comments','<ul><li><strong>Allows users to add comments to the document.</strong></li><li>Other users can view and respond to comments, facilitating discussion and collaboration on the document.</li></ul>','2023-06-03 05:14:57','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-04 14:21:22','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('5d7ba1b1-a380-4e4d-8cb0-56159a6ee0d3','ASSIGNED_DOCUMENTS','Assigned documents','<p>The <strong>\"Assigned Documents\"</strong> page is the central hub for managing documents allocated to a specific user. Here, users can view all the documents assigned to them, search for specific documents, and perform various actions on each document.</p><h3>Main Components:</h3><ul><li><strong>\"Add Document\" Button</strong>: Allows users to upload a new document to the system.<ul><li>Opens a form or pop-up window where files can be selected and uploaded.</li></ul></li><li><strong>My Reminders</strong>: Displays a list of all the reminders set by the user.<ul><li>Users can view, edit, or delete reminders.</li></ul></li><li><strong>Search Box (by name or document)</strong>: Allows users to search for a specific document by entering its name or other document details.</li><li><strong>Search Box (by meta tags)</strong>: Users can enter meta tags to filter and search for specific documents.</li><li><strong>Category Selection Dropdown</strong>: A dropdown menu that allows users to filter documents based on their Category.</li><li><strong>Status Selection Dropdown</strong>: A dropdown menu that allows users to filter documents based on their status.</li><li><strong>List of files allocated to the user</strong>: Displays the documents assigned to the user in allocation order.<ul><li>Each entry includes columns for \"Action,\" \"Name,\" \"Status,\" \"Category Name,\" \"Creation Date,\" \"Expiration Date,\" and \"Created By.\"</li></ul></li><li>Next to each document, there is a menu with options such as \"edit,\" \"share,\" \"view,\" \"upload a version,\" \"version history,\" \"comment,\" and \"add reminder.\"</li></ul><h3>How to Add a New Document:</h3><ol><li>Click the <strong>\"Add Document\"</strong> button.</li><li>A form or pop-up window will open.</li><li>Select and upload the desired file, then fill in the necessary details.</li><li>Click <strong>\"Save\"</strong> or <strong>\"Add\"</strong> to upload the document to the system.</li></ol><h3>How to Search for a Document:</h3><ol><li>Enter the document\'s name or details in the appropriate search box.</li><li>The search results will be displayed in the document list.</li></ol><h3>How to Perform Actions on a Document:</h3><p><strong>Document Action Menu Overview</strong>:<br>The action menu offers users various options for managing and interacting with the assigned documents. Each action is designed to provide specific functionalities, allowing users to work efficiently with their documents.</p><h4>Available Options:</h4><ul><li><strong>Edit</strong>: Allows users to modify the document\'s details, such as its name, description, or meta tags.<ul><li>After making changes, users can save the updates.</li></ul></li><li><strong>Share</strong>: Provides the option to share the document with other users or roles in the system.<ul><li>Users can set specific permissions, such as view or edit, for those with whom the document is shared.</li></ul></li><li><strong>View</strong>: Opens the document in a new window or an embedded viewer, allowing users to view the document\'s content without downloading it.</li><li><strong>Upload a Version</strong>: Allows users to upload an updated version of the document.<ul><li>The original document remains in the system, and the new version is added as an update.</li></ul></li><li><strong>Version History</strong>: Displays all previous versions of the document.<ul><li>Users can view, or download any of the previous versions if the administrator allows the user to download document permission.</li></ul></li><li><strong>Comment</strong>: Allows users to add comments to the document.<ul><li>Other users can view and respond to comments, facilitating discussion and collaboration on the document.</li></ul></li><li><strong>Add Reminder</strong>: Sets a reminder for an event or action related to the document.<ul><li>Users can receive notifications or emails when the reminder date approaches.</li></ul></li></ul>','2023-06-02 17:32:19','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 06:58:47','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('5d858491-f9db-4aef-959f-5af9d7f3b7bd','MANAGE_ROLE','Manage Role','<ul><li>Allows administrators or users with appropriate permissions to create a new role in the system.</li><li>Opens a form or a pop-up window where permissions and role details can be defined.</li><li>Enter the role name and select the appropriate permissions from the available list.</li><li>Click <strong>\"Save\"</strong> or <strong>\"Add\"</strong> to add the role to the system with the specified permissions.</li></ul><p> </p><p><br> </p>','2023-06-03 05:20:37','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-04 14:25:12','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('5ec9fb94-f79e-4b23-bd5b-066001a135d7','FILE_REQUEST','File Request Functionality','<h2>File Request Functionality</h2><p>The <strong>File Request</strong> feature simplifies document collection by allowing you to generate unique links, share them with users, and review uploaded documents. Here\'s how it works:</p><h2>Key Features:</h2><p><strong>1.Generate Link</strong>:</p><ul><li>Create a unique link for a file request.</li><li>Share this link with users to allow them to upload the required documents.</li></ul><p><strong>2.Upload Documents</strong>:</p><p>Users can upload documents directly via the link you provide.</p><p>You can set the following parameters when creating a request:</p><p><strong>Maximum File Size Upload</strong>: Specify the largest file size allowed per upload.</p><p><strong>Maximum Document Upload</strong>: Limit the number of documents a user can upload.</p><ul><li><strong>Allowed File Extensions</strong>: Restrict uploads to specific file types (e.g., PDF, DOCX, JPG).</li></ul><p><strong>3.Review and Manage Requests</strong>:</p><ul><li>View all submissions on the <strong>File Request List</strong> page.</li><li>Approve or reject uploaded documents as necessary.</li></ul><p><strong>4.Request Data List</strong>:<br>Each file request includes the following details:</p><ul><li><strong>Subject</strong>: The purpose or title of the request.</li><li><strong>Email</strong>: The email address associated with the request.</li><li><strong>Maximum File Size Upload</strong>: The size limit for uploaded files.</li><li><strong>Maximum Document Upload</strong>: The number of documents users can upload.</li><li><strong>Allowed File Extensions</strong>: The types of files users can upload.</li><li><strong>Status</strong>: The current status of the request (e.g., Pending, Approved, Rejected).</li><li><strong>Created By</strong>: The user who created the request.</li><li><strong>Created Date</strong>: The date the request was created.</li><li><strong>Link Expiration</strong>: The date the link will no longer be valid.</li></ul><p><strong>5.Manage Requests</strong>:<br>For each file request, you can:</p><ul><li><strong>Edit</strong>: Update the details of the request, such as file size, document limits, or expiration date.</li><li><strong>Delete</strong>: Remove the request entirely.</li><li><strong>Copy Link</strong>: Copy the link to share it with others.</li></ul><h2>How It Works:</h2><h3>1. Creating a File Request:</h3><ul><li>Navigate to the <strong>File Request</strong> page and click \"Create New Request.\"</li><li>Enter details like the subject, allowed file extensions, and upload limits.</li><li>Generate the link and share it with the intended user.</li></ul><h3>2. Uploading Documents:</h3><ul><li>The user clicks the link and uploads their documents according to the criteria you set.</li></ul><h3>3. Reviewing Submissions:</h3><ul><li>Go to the <strong>File Request List</strong> page to view submitted documents.</li><li>Approve or reject submissions as required.</li></ul><h3>4. Managing Links:</h3><ul><li>Use the <strong>Edit</strong> or <strong>Delete</strong> options to modify or remove requests.</li><li>Copy the link anytime for reuse or sharing.</li></ul>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-09 06:19:30','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('6879e107-9109-41fc-8628-9b4937d7baaf','AI_DOCUMENT_GENERATOR','AI Document Generator','<h2>AI Document Generator – <strong>What It Does</strong></h2><p>The <strong>AI Document Generator</strong> helps you quickly create high-quality documents using artificial intelligence (AI).<br>You simply select a prompt template, provide some basic information, and the AI writes the full document for you — ready to <strong>save, share, or download</strong>.</p><h2> <strong>Main Functionalities</strong></h2><h3>1. <strong>Select AI Prompt Template</strong></h3><p>Choose a ready-made writing style or document type (like blog post, email, product description, agreement, etc.)<br>Each template tells the AI what kind of document to create.</p><h3>2. <strong>Fill in Prompt Inputs</strong></h3><p>In the selected template, you’ll see words between double asterisks like **topic** or **productName**.<br>You replace these placeholders with your own information.</p><blockquote><p><strong>Example</strong>:<br>Template: <i>\"Write a detailed article about <strong>topic</strong>.\"</i><br>Your Input: <i>\"Latest Trends in Electric Vehicles\"</i></p></blockquote><h3>3. <strong>Choose Document Settings</strong></h3><ul><li><strong>Language</strong>: Set the language you want the document in (default is English - USA).</li><li><strong>Maximum Length</strong>: Limit how long the output should be.</li><li><strong>Creativity</strong>: Choose if you want the document formal, creative, or somewhere in between.</li><li><strong>Tone of Voice</strong>: Select the style — professional, friendly, persuasive, etc.</li><li><strong>Select Model</strong>: Pick which AI model to use (e.g., GPT-3.5 Turbo).</li></ul><h3>4. <strong>Generate Document</strong></h3><p>Once you fill out the inputs and click <strong>Generate</strong>, the AI will instantly create a professional document for you, based on your selections.</p><h3>5. <strong>Save as PDF</strong></h3><p>After the document is generated, you can <strong>download it as a PDF</strong> file directly from the system.</p><blockquote><p> This is useful for:</p><ul><li>Quickly saving important drafts.</li><li>Sending documents to clients or teams.</li><li>Storing official records.</li></ul></blockquote><h2> <strong>Key Benefits for Users</strong></h2><ul><li>No need to write documents from scratch.</li><li>Easy, fast, and customizable.</li><li>Perfect for business letters, blog posts, agreements, product descriptions, and more.</li><li>Professional-quality output, ready to download as PDF in seconds.</li></ul><h2> <strong>Simple Workflow for Users</strong></h2><ol><li>Select a Template.</li><li>Provide your inputs.</li><li>Set your preferences (language, tone, length).</li><li>Click <strong>Generate</strong>.</li><li>Review the content.</li><li>Click <strong>Save as PDF</strong> — Done!</li></ol>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('6a845385-7964-463c-9064-09efa0ca3c6b','DOCUMENT_SIGNATURE','Document Signature','<p><strong>Document Signature Functionality</strong></p><p>The <strong>Document Signature</strong> feature allows users to digitally sign documents with ease. This functionality is designed to make the process simple, secure, and efficient, eliminating the need for printing and manual signatures.</p><h3>How It Works:</h3><ol><li><strong>Initiating the Signature Process:</strong><ul><li>Users can click on the <strong>\"Document Signature\"</strong> button for any document.</li><li>A <strong>popup window</strong> opens, providing options to add a signature.</li></ul></li><li><strong>Applying the Signature:</strong><ul><li>Users can <strong>draw</strong> their signature using a touchscreen or mouse.</li><li>Alternatively, they can <strong>type</strong> their name and choose from various font styles to create a professional-looking signature.</li><li>The signature can be placed anywhere on the document by dragging it to the desired location.</li></ul></li><li><strong>Additional Functionalities:</strong><ul><li><strong>PDF Signature Integration:</strong> Users can directly sign PDFs without converting file formats.</li><li>The <strong>Company Profile</strong> section allows users to include their company details, such as &nbsp;in the PDF signature.</li></ul></li></ol><h3>Key Features:</h3><ul><li><strong>Interactive and User-Friendly:</strong> The popup makes it easy to apply signatures in just a few clicks.</li><li><strong>Professional Branding:</strong> Integrate company details with your signature for added authenticity.</li><li><strong>Secure Signing:</strong> Digital signatures are encrypted to ensure document integrity.</li><li><strong>Flexibility:</strong> Customize the signature and include additional annotations like dates or initials.</li></ul><h3>Benefits:</h3><ul><li><strong>Streamlined Workflow:</strong> Quickly sign and finalize documents without printing or scanning.</li><li><strong>Enhanced Professionalism:</strong> Signatures with company branding make documents look polished and credible.</li><li><strong>Secure and Reliable:</strong> All signed documents are protected with advanced encryption to ensure they remain tamper-proof.</li></ul><p>With the <strong>Document Signature</strong> feature, signing documents becomes fast, professional, and secure, offering users the flexibility and tools they need to manage their documents seamlessly.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-28 07:44:00','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('6dd7fdb8-efa2-46f7-ae22-ac24630c31c0','TABLE_SETTINGS','Table Settings','<p>The table provides an interactive way for users to customize the display of data. You can:</p><p>✅ Rearrange Columns – Drag and drop columns to change their order.<br>✅ Resize Columns – Adjust the width of each column by dragging its edges.<br>✅ Show/Hide Columns – Use a settings menu to select which columns to display or hide.<br>✅ Persistent Settings – Your preferences (order, visibility, and width) can be saved for future use.</p><p>This customization ensures that you see only the relevant information in the way that suits you best.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('762a5894-0c49-48d8-9e0c-e5062a4c3322','SEND_EMAIL','Send mail','<ul><li><strong>How to Send a Document as an Email Attachment:</strong></li><li><strong>Select the email field</strong>: Navigate to the section where you can compose an email and select the field for entering the recipient\'s email address.</li><li><strong>Enter the email address</strong>: Type the recipient\'s email address in the provided field.</li><li><strong>Subject field</strong>: Enter a relevant subject for your email.</li><li><strong>Email content</strong>: Write the body of your email, providing any necessary context or information.</li><li><strong>Attach the document</strong>: Find the option to \"Attach\" or \"Upload\" a document, then select the file you wish to send.</li><li><strong>Send the email</strong>: After attaching the document and ensuring the recipient, subject, and content are correct, click the \"Send\" button to deliver the email with the attached document.</li></ul>','2023-06-03 05:16:00','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 06:16:52','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('7b831b4c-2357-4757-8914-e894aa651fbc','RECENT_ACTIVITY','Recent Activity','<p><strong>General Description:</strong></p><p>The \"Recent Activity\" page provides a detailed view of all actions performed on documents within the DMS. It allows administrators and users with appropriate permissions to monitor and review document-related activities, ensuring transparency and information security.</p><p><strong>Main Components:</strong></p><p><strong>Search Boxes:</strong></p><ul><li><strong>By Document Name:</strong> Allows users to search for a specific document by entering its name or other details.</li><li><strong>By Meta Tag:</strong> Users can enter meta tags to filter and search for specific document-related activities.</li><li><strong>By User:</strong> Enables filtering activities based on the user who performed the operation.</li></ul><p><strong>List of Audited Documents:</strong></p><p>Displays all actions taken on documents in a tabular format.</p><p>Each entry includes details of the action, such as the date, document name, category, operation performed, who performed the operation, to which user, and to which role the operation was directed.</p><p>Users can click on an entry to view additional details or access the associated document.</p><p><strong>List Sorting:</strong></p><p>Users can sort the list by any of the available columns, such as \"Date,\" \"Name,\" \"Category Name,\" \"Operation,\" \"Performed by,\" \"Directed to User,\" and \"Directed to Role.\"</p><p>This feature makes it easier to organize and analyze information based on specific criteria.</p><p><strong>How to Search the Audit History:</strong></p><ul><li>Enter your search criteria in the corresponding search box (document name, meta tag, or user).</li><li>The search results will be displayed in the audited documents list.</li></ul><p><strong>How to Sort the List:</strong></p><ul><li>Click on the column title by which you want to sort the list (e.g., \"Date\" or \"Name\").</li><li>The list will automatically reorder based on the selected criterion.</li></ul>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('8c1e5b05-0d7e-45cc-973d-423b2e10c5fd','SHARE_DOCUMENT','Share Document','<h4>Overview</h4><p>The <strong>Share Document</strong> feature allows users to assign access permissions to specific documents for individual users or user roles, with the ability to manage these permissions effectively. Users can also remove existing permissions, enhancing collaboration and control over document access.</p><h4>Features</h4><ol><li><strong>Assign By Users and Assign By Roles</strong><ul><li><strong>Buttons:</strong><ul><li>Two separate buttons are available at the Top of the share document section:<ul><li><strong>Assign By Users:</strong> Opens a dialog for selecting individual users to share the document with.</li><li><strong>Assign By Roles:</strong> Opens a dialog for selecting user roles to share the document with.</li></ul></li></ul></li><li><strong>User/Roles List:</strong><ul><li>Below the buttons, a list displays users or roles who currently have document permissions, including details such as:</li><li>Delete Button( Allow to delete existing permission to user or role)<ul><li>User/Role Name</li><li>Type (User/Role)</li><li>Allow Download(if applicable)</li><li>Email(if applicable)</li><li>Start Date (if applicable)</li><li>End Date (if applicable)</li><li> </li><li><strong>Delete Button:</strong> A delete button next to each user/role in the list, allowing for easy removal of permissions.</li></ul></li></ul></li></ul></li><li><strong>Dialog for Selection</strong><ul><li><strong>Dialog Features:</strong><ul><li>Upon clicking either <strong>Assign By Users</strong> or <strong>Assign By Roles</strong>, a dialog opens with the following features:<ul><li><strong>User/Role Selection:</strong><ul><li>A multi-select dropdown list allows users to select multiple users or roles for sharing the document.</li></ul></li><li><strong>Additional Options:</strong><ul><li><strong>Share Duration:</strong> Users can specify a time period for which the document will be accessible (e.g., start date and end date). </li><li><strong>Allow Download:</strong> A checkbox option that allows users to enable or disable downloading of the document.</li><li><strong>Allow Email Notification:</strong>A checkbox option that, when checked, sends an email notification to the selected users/roles.<ul><li>If this option is selected, SMTP configuration must be set up in the application. If SMTP is not configured, an error message will display informing the user of the missing configuration.</li></ul></li></ul></li></ul></li></ul></li></ul></li><li><strong>Saving Shared Document Permissions</strong><ul><li><strong>Save Button:</strong><ul><li>A <strong>Save</strong> button within the dialog allows users to save the selected permissions.</li></ul></li><li><strong>Reflection of Changes:</strong><ul><li>Upon saving, the data is updated, and the list at the bottom of the main interface reflects the newly shared document permissions, showing:<ul><li>User/Role Name</li><li>Type (User/Role)</li><li>Allow Download(if applicable)</li><li>Email(if applicable)</li><li>Start Date (if applicable)</li><li>End Date (if applicable)</li><li>Whether download and email notification options are enabled</li></ul></li></ul></li></ul></li><li><strong>Removing Shared Permissions</strong><ul><li><strong>Delete Button Functionality:</strong><ul><li>Users can click the <strong>Delete</strong> button next to any user or role in the existing shared permissions list.</li><li><strong>Confirmation Dialog:</strong> A confirmation prompt appears to ensure that users intend to remove the selected permission. Users must confirm the action to proceed.</li></ul></li><li><strong>Updating the List:</strong><ul><li>Once confirmed, the shared permission for the selected user or role is removed from the list, and the list updates immediately to reflect this change.</li></ul></li></ul></li><li><strong>User Interaction Flow</strong><ul><li><strong>Navigating to Share Document:</strong><ul><li>Users access the <strong>Share Document</strong> section within the application.</li></ul></li><li><strong>Assigning Permissions:</strong><ul><li>Users click on <strong>Assign By Users</strong> or <strong>Assign By Roles</strong> to open the respective dialog.</li><li>They select the appropriate users or roles, configure additional options, and click <strong>Save</strong>.</li></ul></li><li><strong>Removing Permissions:</strong><ul><li>Users can remove permissions by clicking the <strong>Delete</strong> button next to an entry in the shared permissions list and confirming the action.</li></ul></li><li><strong>Reviewing Shared Permissions:</strong><ul><li>The updated list displays the current permissions, allowing users to verify and manage document sharing effectively.</li></ul></li></ul></li></ol><h3>Summary</h3><p>The <strong>Share Document</strong> functionality provides a structured interface for assigning and managing document permissions to individual users or roles, with added flexibility to remove existing permissions. This feature enhances document collaboration and control while ensuring users can efficiently manage access. The inclusion of SMTP configuration checks for email notifications adds robustness to the communication aspect of the document-sharing process.</p>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 09:44:37','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('8d114f7f-6500-4c33-87c3-d896e2132bcc','MANAGE_FILE_REQUEST','Manage File Request','<p>The <strong>Manage File Request</strong> feature allows you to create, edit, and control your file requests efficiently. This includes setting security options such as a password and defining a link expiration time. Here’s how it works:</p><h2>Key Features:</h2><p><strong>1.Add New File Request</strong>:</p><ul><li>Create a new file request by providing details like:</li></ul><p><strong>Subject</strong>: A clear title for the request.</p><p><strong>Email</strong>: The email address associated with the request.</p><p><strong>Maximum File Size Upload</strong>: Define the largest file size allowed per upload.</p><p><strong>Maximum Document Upload</strong>: Set the maximum number of documents users can upload.</p><p><strong>Allowed File Extensions</strong>: Restrict uploads to specific file types (e.g., PDF, DOCX, JPG).</p><p><strong>Password Protection</strong>: Add a password to secure the upload link.</p><p><strong>Link Expiry Time</strong>: Set a date and time after which the link will no longer be valid.</p><p><strong>2.Edit File Request</strong>:</p><ul><li>Update existing file requests to adjust settings as needed:<ul><li>Change the <strong>Subject</strong>, <strong>Email</strong>, or upload criteria.</li><li>Modify the <strong>Password</strong> or disable it.</li><li>Extend or shorten the <strong>Link Expiry Time</strong>.</li></ul></li></ul><p><strong>3.Secure Uploads</strong>:</p><ul><li>By setting a password, only users with the password can upload files, ensuring additional security.</li></ul><p><strong>4.Link Expiration</strong>:</p><ul><li>The link will automatically expire after the set date and time, preventing further uploads.</li></ul><p><strong>5.Manage File Requests List</strong>:</p><ul><li>View and manage all file requests from a centralized list with the following options:</li></ul><p><strong>Edit</strong>: Update the details of an existing file request.</p><p><strong>Delete</strong>: Remove a file request permanently.</p><p><strong>Copy Link</strong>: Copy the unique link to share it with others.</p><h2>How It Works:</h2><h3>1. Adding a File Request:</h3><ul><li>Go to the <strong>File Request</strong> page and click \"Add New Request.\"</li><li>Fill in the following details:</li></ul><p><strong>Subject</strong>: The purpose of the request.</p><p><strong>Email</strong>: The email address for communication.</p><p><strong>Maximum File Size Upload</strong> and <strong>Maximum Document Upload</strong>.</p><p><strong>Allowed File Extensions</strong>: Specify the types of files you will accept.</p><p><strong>Password Protection</strong>: Create a password to restrict access.</p><p><strong>Link Expiry Time</strong>: Set a specific date and time for the link to expire.</p><ul><li>Save the request to generate the link.</li></ul><h3>2. Editing a File Request:</h3><ul><li>Open the <strong>File Request List</strong> and locate the request you want to modify.</li><li>Click the <strong>Edit</strong> button.</li><li>Adjust any of the request details, such as upload limits, password, or expiry time.</li><li>Save the changes.</li></ul><h3>3. Sharing and Using the Link:</h3><ul><li>Copy the generated link and share it with the intended users.</li><li>If a password is set, ensure the recipient has the password to access the link.</li></ul><h3>4. Expired Links:</h3><ul><li>Once the link expires, users will no longer be able to upload files.</li><li>Extend the expiry time if additional uploads are required by editing the request.</li></ul>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-09 06:24:14','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('8fa4ec82-af7e-4182-b413-d38eb9a55a9a','CLIENTS','Clients','<p>The <strong>Clients</strong> section helps you manage and view all your clients in one place. Here’s what you can do:</p><p><strong>1.Clients List</strong></p><ul><li>A list of all your clients is displayed with the following details:</li></ul><p><strong>Action</strong>: Options to edit or delete client information.</p><p><strong>Company/Person Name</strong>: The name of the company or individual client.</p><p><strong>Contact Person</strong>: The primary contact person for the client.</p><p><strong>Email</strong>: The email address of the client for communication.</p><p><strong>Mobile Number</strong>: The mobile number of the client for easy contact.</p><p><strong>2.Add Client</strong></p><ul><li>Click the <strong>\"Add Client\"</strong> button to create a new client.</li><li>Fill in details like the company or person name, contact person, email, and mobile number.</li><li>Save the new client, and it will be added to the clients list.</li></ul>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-10 05:08:33','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('8fd8c3c6-ac6e-45d8-9757-00300fc9ca8f','DOCUMENT_STATUS','Document Status','<p>Users can add, edit, and view a list of document statuses. Each status includes three customizable fields: name, description, and a unique colour code for easy identification and organization of documents.</p><h4><strong>Main Components:</strong></h4><p><strong>\"Add New Document Status\" Button:</strong></p><p>Allows administrators or users with appropriate permissions to create a new status.</p><ul><li><strong>List of Existing Statuses:</strong></li><li>Displays all the Statuses created within the system.</li><li>Each entry includes the status name, description and unique colour code for easy identification and organization of documents.</li><li><strong>Action Menu for Each Status:</strong></li><li>Next to each status, users will find action options that allow them to manage the Category:<ul><li><strong>Edit:</strong> Enables modification of the status\'s details, such as the name or description, and unique colour code .</li><li><strong>Delete:</strong> Removes the status from the system. This action may require confirmation to prevent accidental deletions.</li></ul></li></ul>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 07:18:42','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('9a30dbb2-811b-4e2a-aea1-99ff1fe1ccc2','WORKFLOW_LOGS','Workflow Logs','<ul><li><h3>Workflow Logs Page Overview</h3></li><li>The <strong>Workflow Logs Page</strong> provides a comprehensive log of every step and transition within a workflow. It helps users track the detailed progression of workflows, offering transparency and visibility into the actions and decisions made at each stage.</li><li><h4><strong>Key Features</strong></h4></li><li><strong>Detailed Step and Transition Logs</strong>:<ul><li>Every step and transition in the workflow is logged with the following details:<ul><li><strong>Name</strong>: The name of the workflow step or transition.</li><li><strong>Status</strong>: Current status of the step or transition (e.g., Completed, Pending, In Progress).</li><li><strong>Initiated By</strong>: The user who initiated the step or transition.</li><li><strong>Initiated Date</strong>: The date and time when the step or transition was initiated.</li><li><strong>Document Name</strong>: The name of the document associated with the workflow.</li><li><strong>Transition Name</strong>: The name of the transition between steps.</li><li><strong>Steps</strong>: The specific step(s) in the workflow.</li><li><strong>Transition Status</strong>: Status of the transition (e.g., Completed, Pending).</li><li><strong>Performed By</strong>: The user or team who performed the transition or step.</li><li><strong>Transition Date</strong>: The date and time when the transition occurred.</li><li><strong>Comment</strong>: Any additional comments or notes added regarding the step or transition.</li></ul></li></ul></li><li><strong>Complete Workflow Tracking</strong>:<ul><li>The logs track each workflow step from start to finish, offering a complete audit trail of all actions taken during the workflow\'s lifecycle.</li><li>Users can quickly see when each step was initiated, who performed the actions, and if any comments were added, providing full insight into the workflow\'s progress and changes.</li></ul></li><li><strong>Easy Navigation and Filtering</strong>:<ul><li>The log entries are organized and displayed in a table format, making it easy for users to scroll through, filter, or search for specific steps or transitions.</li><li>Users can filter by workflow name, status, initiated date, transition date, and other fields to find the exact log details they need.</li></ul></li><li><h3>Benefits</h3></li><li>The <strong>Workflow Logs Page</strong> provides an in-depth look at the execution of each workflow, ensuring that users can:</li><li><strong>Track Workflow Progress</strong>: View every step and transition with timestamps and status updates.</li><li><strong>Maintain Transparency</strong>: Ensure all actions are recorded with details about who performed each step and when.</li><li><strong>Access Full Audit Trails</strong>: Review comments, dates, and statuses to verify workflow history and identify areas for improvement.</li><li>This page is crucial for monitoring and reviewing workflows, offering full visibility into their progression and actions taken at every stage.</li></ul>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-20 06:15:23','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('9bb0fa5e-5386-4132-9b67-8e83fd0f5acd','BULK_DOCUMENT_UPLOADS','Bulk Document Uploads','<h3><strong>Bulk Document Uploads</strong></h3><p>Easily upload multiple documents to your system with the following steps:</p><p><strong>1.Category</strong></p><ul><li><strong>Select Category</strong>: Choose a category where your documents will be stored. This helps organize your uploads.</li></ul><p><strong>2.Document Status</strong></p><ul><li>Define the status of each document (e.g., Draft, Final, Archived). This ensures clarity and organization.</li></ul><p><strong>3.Storage</strong></p><ul><li>Select the storage location for your documents:<ul><li><strong>Local</strong>: Save documents to the local storage system.</li></ul></li></ul><p><strong>4.Assign By Roles</strong></p><ul><li><strong>5.Roles</strong>: Assign specific roles to the documents. For example: \"Manager,\" \"Editor,\" or \"Viewer.\"</li><li>This determines which roles have access to the uploaded documents.</li></ul><p><strong>6.Assign By Users</strong></p><ul><li><strong>7.Users</strong>: Assign individual users who can access these documents.</li><li>Select from a list of users in your system.</li></ul><p><strong>8.Document Upload</strong></p><ul><li>Select multiple files to upload from your device.</li><li>Ensure the file extensions are in the allowed list.</li><li>Optionally, rename files before uploading to keep them organized.</li></ul><p><strong>9.Finalize Upload</strong></p><ul><li>After filling out all the required fields, upload the documents.</li><li>The system will automatically assign the selected roles and users to each uploaded file.</li></ul>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-10 05:08:33','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('a1c28412-9590-4cdb-b7a0-1687e890ad5d','ADD_REMINDER','Add reminder','<p><strong>The \"Add Reminder\" functionality in the \"Manage Reminders\" section allows users to create reminders or notifications related to specific events or tasks. These reminders can be customized according to the user\'s needs and can be sent to specific other users.</strong></p><p><strong>Components and Features:</strong></p><ul><li><strong>Subject:</strong> This field allows the user to enter a title or theme for the reminder. This will be the main subject of the notification.</li><li><strong>Message:</strong> Here, users can add additional details or information related to the reminder. This can be a descriptive message or specific instructions.</li><li><strong>Repeat Reminder:</strong> This option allows setting the frequency with which the reminder will be repeated, such as daily, weekly, or monthly.</li><li><strong>Send Email:</strong> If this option is enabled, the reminder will also be sent as an email to the selected users.</li><li><strong>Select Users:</strong> This field allows the selection of users to whom the reminder will be sent. Users can be selected individually or in groups.</li><li><strong>Reminder Date:</strong> This is the time at which the reminder will be activated and sent to the selected users.</li></ul><p><strong>How to Add a New Reminder:</strong></p><ul><li>; to the \"Manage Reminders\" section.</li><li>Click the \"Add Reminder\" button.</li><li>Fill in all required fields with the desired information.</li><li>After entering all the details, click \"Save\" or \"Confirm\" to add the reminder to the system.</li></ul>','2023-06-03 05:09:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 05:17:00','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('a3664127-34f1-494c-84c5-fc3f307a9d11','USER_PAGE_PERMISSION_TO','User Page Permission To','<ul><li>Enable the ability to assign specific permissions to users that are not tied to their assigned roles. This gives admins the flexibility to grant access to particular features for individual users.</li><li>Click <strong>\"Save\"</strong> or <strong>\"Add\"</strong> to assign the user to the system with the specified permissions.</li></ul>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','0001-01-01 00:00:00','00000000-0000-0000-0000-000000000000',NULL,NULL,0),('a574a57c-f528-4a52-b5d5-a1a0c4e5ada1','MANAGE_DOCUMENT_META_TAG','Manage Document Meta Tags','<p>Users can <strong>add, edit, and manage</strong> document meta tags to customize metadata for documents.</p><h4><strong>Features:</strong></h4><p>✅ <strong>Add Meta Tags</strong>: Users can create new meta tags by specifying:</p><ul><li><strong>Type</strong> (e.g., Text, Date, Number)</li><li><strong>Name</strong> (Custom field name)</li><li><strong>Is Editable</strong> (Allows modification if enabled)</li></ul><p>✅ <strong>Edit Meta Tags</strong>: Users can update the <strong>Type</strong> and <strong>Name</strong> if the tag is marked as <strong>Editable</strong>.</p><p>✅ <strong>Manage Meta Tags</strong>: A structured table displays existing meta tags with options to edit (if allowed) or remove them.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('A8209505-ED74-4D3A-93F4-8185192994DC','DEEP_SEARCH','Deep Search','<p>The <strong>Deep Search</strong> feature allows you to search through the content of various types of documents, including Word documents, PDFs, Notepad files, PPT, Images (.tif, .tiff, .png, .jpg, .jpeg, .bmp, .pbm, .pgm, .ppm) and Excel spreadsheets. Follow the instructions below to ensure accurate and efficient searches.</p><h4><strong>1. How to Perform a Search</strong></h4><ul><li><strong>Basic Search</strong>: Enter keywords or phrases in the search bar. The system will look for these terms across supported document types. When the \'Search Exact Match\' checkbox is checked, the system will search for documents that contain the exact phrase entered in the search field. </li></ul><h4><strong>2. Case Sensitivity</strong></h4><ul><li><strong>Case-Insensitive Search</strong>: The search is not case-sensitive. This means that searching for \"Report\" and \"Report\" will return the same results, regardless of capitalization.</li></ul><h4><strong>3. Common Words</strong></h4><ul><li><strong>Ignored Words</strong>: Common words such as \"and,\" \"the,\" and \"is\" are automatically ignored to provide more relevant results. This improves search accuracy by focusing on significant terms in your query.</li></ul><h4><strong>4. Word Variations (Stemming)</strong></h4><ul><li><strong>Word Variations</strong>: The search automatically includes variations of the words you enter. For example, if you search for \"run\", the system will also return documents that contain \"running\", \"runs\", and similar variations.</li></ul><h4><strong>5. Supported File Types</strong></h4><p>Deep Search can find content within the following document types:</p><ul><li><strong>Microsoft Word</strong> documents (.doc, .docx)</li><li><strong>Writable PDF</strong> files (editable, non-scanned PDFs)</li><li><strong>Notepad</strong> text files (.txt)</li><li><strong>Excel spreadsheets</strong> (.xls, .xlsx)</li><li><strong>PowerPoint documents</strong> (.ppt, .pptx)</li><li><strong>Images </strong>(tif,.tiff,.png,.jpg,.jpeg,.bmp,.pbm,.pgm,.ppm)</li></ul><p>Ensure that your documents are in one of these supported formats for the search function to work properly.</p><h4><strong>6. Search Results Limit</strong></h4><ul><li>You will receive a <strong>maximum of 10 results</strong> per search. If more documents match your query, you may need to refine your search to narrow down the results.</li></ul><h4><strong>7. Indexing Time for New Documents</strong></h4><ul><li>After uploading a new document, it may take <strong>15 to 20 minutes</strong> for it to become searchable. This delay is due to background document indexing, which is necessary to prepare the document\'s content for Deep Search.</li><li> </li><li><h4><strong>8. Removing a Document from Indexing</strong></h4></li><li>If you no longer want a document to be searchable, you can <strong>remove it from indexing</strong>. Follow these steps:</li><li>Navigate to the <strong>Document List</strong> page.</li><li>Find the document you want to remove from search indexing.</li><li>Click on the <strong>Remove Page Indexing</strong> menu item for that document.</li><li>After removal, the document\'s content will no longer be searchable. <strong>Once removed, the document will not appear in search results, and the indexing process cannot be reversed.</strong></li></ul><h4><strong>9. Tips for Better Search Results</strong></h4><ul><li>Use <strong>specific keywords</strong>: The more specific your search terms, the more relevant the results will be.</li><li>Combine <strong>exact phrase search</strong> and regular search terms to narrow down results (e.g., \"annual report\" budget overview).</li><li>Avoid using common words that the system ignores unless they are part of an exact phrase search.</li></ul>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','0001-01-01 00:00:00','00000000-0000-0000-0000-000000000000',NULL,NULL,0),('aac91fd9-c6df-4c75-937b-775dbfbdc0d7','WORKFLOW_SETTINGS','Workflow Settings','<ul><li><h3><strong>Workflow Settings Overview</strong></h3></li><li>The <strong>Workflow Settings</strong> allow users to create and configure workflows in a structured and straightforward manner. The process is divided into three main steps to ensure ease of understanding and use:</li><li><h4><strong>Step 1: Define Workflow Details</strong></h4></li><li>In this step, users provide the foundational details for their workflow:</li><li><strong>Workflow Name</strong>: Enter a unique and descriptive name for the workflow.</li><li><strong>Description</strong>: Add a brief description to explain the purpose or functionality of the workflow.</li><li>Once these details are entered, users can proceed to the next step.</li><li><h4><strong>Step 2: Create Workflow Steps</strong></h4></li><li>In the second step, users define the specific steps involved in the workflow:</li><li><strong>Add Workflow Steps</strong>: Create individual steps that represent the actions or processes in the workflow.</li><li>Arrange and name the steps according to the workflow’s logic or sequence.</li><li>After defining all the necessary steps, users can move to the final step.</li><li><h4><strong>Step 3: Configure Workflow Transitions</strong></h4></li><li>In this step, users set up the rules and connections between workflow steps:</li><li><strong>Define Transitions</strong>: Specify how the workflow moves from one step to another (e.g., conditions or triggers for moving to the next step).</li><li>Ensure the transitions align with the desired workflow logic.</li></ul>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-20 05:51:18','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('ab28cf5c-0d89-4a52-a87b-359106897cba','MANAGE_EMAIL_SMTP_SETTING','Manage Email SMTP Setting','<p>The <strong>\"Email SMTP Settings\"</strong> page within CMR DMS allows administrators to configure and manage the SMTP settings for sending emails. This ensures that emails sent from the system are correctly and efficiently delivered to recipients.</p><p><strong>Key Components:</strong></p><ul><li><p><strong>SMTP Settings Table:</strong> Displays all configured SMTP settings in a tabular format.</p><p>Each entry in the table includes details such as the username, host, port, and whether that configuration is set as the default.</p></li><li><p><strong>\"Add Settings\" Button:</strong> Allows administrators to add a new SMTP configuration.</p><p>Clicking the button opens a form where details like username, host, port, and the option to set it as the default configuration can be entered.</p></li></ul><p><strong>\"Add Settings\" Form:</strong></p><p>This form opens when administrators click the \"Add Settings\" button and includes the following fields:</p><ul><li><strong>Username:</strong> The username required for authentication on the SMTP server.</li><li><strong>Host:</strong> The SMTP server address.</li><li><strong>Port:</strong> The port on which the SMTP server listens.</li><li><strong>Is Default:</strong> A checkbox that allows setting this configuration as the default for sending emails.</li></ul><p><strong>How to Add a New SMTP Configuration:</strong></p><ol><li>Click the \"Add Settings\" button.</li><li>The \"Add Settings\" form will open, where you can enter the SMTP configuration details.</li><li>Fill in the necessary fields and select the desired options.</li><li>After completing the details, click \"Save\" or \"Add\" to add the configuration to the system.</li></ol>','2023-06-03 05:27:13','4b352b37-332a-40c6-ab05-e38fcf109719','0001-01-01 00:00:00','00000000-0000-0000-0000-000000000000',NULL,NULL,0),('af147be0-b4ae-48ba-aa81-2dc3cb106c6e','MANAGE_WORKFLOW','Manage Workflow','<ul><li><h3><strong>Manage Workflow Overview</strong></h3></li><li>The <strong>Manage Workflow</strong> feature allows users to efficiently create, edit, and customize workflows as needed. This functionality is designed to ensure flexibility and control over workflow management. Here\'s how it works:</li><li><h4><strong>Creating a Workflow</strong></h4></li><li>If no workflows have been created, users can start by building a new workflow:</li><li><strong>Define Workflow Details</strong>: Provide a unique name and description for the workflow.</li><li><strong>Add Workflow Steps</strong>: Create the necessary steps that outline the workflow process.</li><li><strong>Set Workflow Transitions</strong>: Define the transitions between steps, specifying conditions or rules for movement.</li><li>Once the workflow is created, users can manage and update it as required.</li><li><h4><strong>Editing an Existing Workflow</strong></h4></li><li>For workflows that have already been created, users have the ability to make updates:</li><li><strong>Edit Workflow Name</strong>: Change the name of the workflow to reflect new requirements or corrections.</li><li><strong>Edit Workflow Step Name</strong>: Modify the names of individual steps within the workflow to ensure clarity or adjust for changes.</li><li><strong>Edit Workflow Transition Name</strong>: Update the names or rules for transitions between workflow steps as needed.</li><li><h3>Flexibility in Management</h3></li><li>The <strong>Manage Workflow</strong> feature is versatile, allowing users to either:</li><li><strong>Create a new workflow</strong> if none exist, or</li><li><strong>Edit an existing workflow</strong> to adapt to evolving needs.</li></ul>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-20 05:50:49','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('b1c70caf-ce26-4dff-8f8a-aed4c8eab097','PAGE_HELPERS','Page Helpers','<p>Users can manage the pages within the application using a user-friendly interface that displays a list of available pages. Each entry in the list includes options to <strong>Edit</strong> or <strong>View</strong> the corresponding page\'s details.</p><h4>Features</h4><ol><li><h4><strong>List of Pages</strong></h4><ul><li>Users can see a comprehensive list of all pages in the application, each with the following details:<ul><li><strong>Unique Code:</strong> A non-editable code for each page.</li><li><strong>Editable Name:</strong> An editable field that allows users to change the page name.</li><li><strong>Page Info Content:</strong> A section that displays the functionality description of each page.</li><li> </li></ul></li></ul></li><li><h4><strong>Edit Feature</strong></h4><ul><li><strong>Edit Button:</strong><ul><li>When a user clicks the <strong>Edit</strong> button next to a page, they are directed to an editable form.</li><li>Users can modify the page name and update the page info content to reflect any changes or improvements.</li><li><strong>Validation:</strong><ul><li>The form includes validation checks to ensure that the new name is unique and meets any defined requirements (e.g., length, special characters).</li></ul></li><li><strong>Save Changes:</strong><ul><li>Users can save the changes, which are then reflected in the list of pages and will persist across sessions.</li><li> </li></ul></li></ul></li></ul></li><li><h4><strong>View Feature</strong></h4><ul><li><strong>View Button:</strong><ul><li>Clicking the <strong>View</strong> button opens a dialog box displaying a preview of the page info content.</li><li>This preview includes  current page name, and detailed functionality description.</li><li><strong>Modal Dialog:</strong><ul><li>The dialog box is modal, meaning users cannot interact with the rest of the application until they close the dialog.</li><li>Users can close the dialog by clicking an \"X\" button or a \"Close\" button.</li></ul></li></ul></li></ul></li><li> <ul><li><h4><strong>Navigating to the Page List:</strong></h4><ul><li>Users can easily navigate to the page list through the main navigation menu.</li></ul></li><li><strong>Editing a Page:</strong><ul><li>Users select the <strong>Edit</strong> button next to the desired page, modify the name and content, and click <strong>Save</strong> to apply the changes.</li></ul></li><li><strong>Viewing a Page:</strong><ul><li>Users can click the <strong>View</strong> button to open the dialog box, review the details, and close the dialog when finished.</li></ul></li></ul></li></ol><h3>Summary</h3><p>This functionality empowers users to effectively manage page names and content within the application, ensuring that information is accurate and up-to-date. The combination of edit and view features enhances the user experience by allowing for quick modifications and easy access to page details.</p>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 08:53:15','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('b99e45c1-9d9f-4b0e-80f0-906c7c830394','STORAGE_SETTINGS','Storage Settings','<p><strong>Document Storage Settings</strong>:<br>Users can configure various storage options, including AWS S3 and Cloudflare R2, with specific fields required for each storage type. Additionally, there is a default option available for storing files on a local server. This local server setting cannot be deleted, ensuring a reliable and consistent storage option for users.</p><ol><li><strong>Enable Encryption</strong>: When selected, this option ensures that files are stored in encrypted form within the chosen storage.</li><li><strong>Set as Default</strong>: If this option is set to \"true,\" the storage becomes the default selection in the dropdown on the document add page.</li></ol><p>Upon saving the storage settings, the system attempts to upload a dummy file to verify the configuration. If the upload is successful, the settings are saved; otherwise, an error message prompts the user to adjust the field values.</p><ul><li><h4><strong>Add a new Storage Setting to the system.</strong></h4></li><li><strong>It includes the following fields:</strong></li><li><strong>Storage Type: </strong>AWS/CloudFlare-R2</li><li><strong>Access Key:</strong></li><li><strong>Secret Key:</strong></li><li><strong>Bucket Name:</strong></li><li><strong>Account ID: </strong>Required for CloudFlare-R2 Storage Type</li><li><strong>Enable Encryption: </strong>When selected, this option ensures that files are stored in encrypted form within the chosen storage.</li><li><strong>Is Default:</strong>  If this option is set to \"true,\" the storage becomes the default selection in the dropdown on the document add page.</li><li> </li><li><h4><strong>Edit Storage Setting to the system.</strong></h4></li><li>Users can edit existing storage settings from the storage settings list, which includes an edit button on the left side of each row. When the edit button is clicked, the row opens in edit mode, allowing users to modify the following fields: name, \"Is Default,\" and \"Enable Encryption.\" This provides users with the flexibility to update their storage configurations as needed.</li></ul><h4>CREATE AWS S3 ACCOUNT:</h4><p><a href=\"https://aws.amazon.com/free/?gclid=CjwKCAjwx4O4BhAnEiwA42SbVPBXf7hpN07vHx4ObiZX3xFHpgCLP9mHQ4P1CaykaQkJKT53F2EQFhoCWRkQAvD_BwE&trk=b8b87cd7-09b8-4229-a529-91943319b8f5&sc_channel=ps&ef_id=CjwKCAjwx4O4BhAnEiwA42SbVPBXf7hpN07vHx4ObiZX3xFHpgCLP9mHQ4P1CaykaQkJKT53F2EQFhoCWRkQAvD_BwE:G:s&s_kwcid=AL!4422!3!536324516040!e!!g!!aws%20s3%20account!11539706604!115473954714&all-free-tier.sort-by=item.additionalFields.SortRank&all-free-tier.sort-order=asc&awsf.Free%20Tier%20Types=*all&awsf.Free%20Tier%20Categories=*all\">https://aws.amazon.com/free/?gclid=CjwKCAjwx4O4BhAnEiwA42SbVPBXf7hpN07vHx4ObiZX3xFHpgCLP9mHQ4P1CaykaQkJKT53F2EQFhoCWRkQAvD_BwE&trk=b8b87cd7-09b8-4229-a529-91943319b8f5&sc_channel=ps&ef_id=CjwKCAjwx4O4BhAnEiwA42SbVPBXf7hpN07vHx4ObiZX3xFHpgCLP9mHQ4P1CaykaQkJKT53F2EQFhoCWRkQAvD_BwE:G:s&s_kwcid=AL!4422!3!536324516040!e!!g!!aws%20s3%20account!11539706604!115473954714&all-free-tier.sort-by=item.additionalFields.SortRank&all-free-tier.sort-order=asc&awsf.Free%20Tier%20Types=*all&awsf.Free%20Tier%20Categories=*all</a></p><h4><strong>CREATE  Cloudflare R2</strong></h4><p><a href=\"https://developers.cloudflare.com/workers/tutorials/upload-assets-with-r2/\">https://developers.cloudflare.com/workers/tutorials/upload-assets-with-r2/</a></p><ul><li> </li></ul>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 08:46:32','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('c1911262-774f-4be8-9471-455ba4e19b7d','WORKFLOWS','Workflows','<ul><li><h3>Workflow List Page Overview</h3></li><li>The <strong>Workflow List Page</strong> provides a complete overview of all workflows, displaying their statuses and details to help users manage and monitor workflows effectively. It combines visual graphs and detailed information to ensure clarity and usability.</li><li><h4><strong>Key Features</strong></h4></li><li><strong>Comprehensive Workflow Display</strong>:<ul><li>All workflows are listed on this page, categorized by their statuses:<ul><li><strong>Completed</strong>: Workflows that have been fully executed.</li><li><strong>Initiated</strong>: Newly started workflows awaiting progress.</li><li><strong>In Progress</strong>: Ongoing workflows with steps and transitions in process.</li><li><strong>Cancelled</strong>: Workflows that were terminated before completion.</li></ul></li></ul></li><li><strong>Workflow Details in Graphical View</strong>:<ul><li>Workflows are visually represented using graphs, showcasing:<ul><li>The structure of steps and transitions.</li><li><strong>Completed Transitions</strong>: Clearly highlighted.</li><li><strong>Pending Transitions</strong>: Distinctly marked.</li></ul></li><li>This graphical format allows users to quickly understand the workflow’s progress and flow.</li></ul></li><li><strong>Workflow Information Table</strong>:<ul><li>Each workflow is accompanied by a table containing detailed information:<ul><li><strong>Workflow Name</strong>: Unique name of the workflow.</li><li><strong>Workflow Status</strong>: Current status of the workflow (Completed, Initiated, In Progress, Cancelled).</li><li><strong>Initiated By</strong>: The user who initiated the workflow.</li><li><strong>Document Name</strong>: The associated document, if applicable.</li><li><strong>Workflow Step</strong>: The current step(s) in the workflow.</li><li><strong>Workflow Step Status</strong>: Status of each step (Completed, Pending).</li><li><strong>Performed By</strong>: The user or team responsible for a specific step.</li><li><strong>Transition Status</strong>: Indicates the progress of transitions (Completed or Pending).</li></ul></li></ul></li><li><strong>Interactive Details</strong>:<ul><li>Users can click on workflow steps or transitions in the graph or table to access:<ul><li>Detailed descriptions.</li><li>Status history.</li><li>Timestamps and related actions.</li></ul></li></ul></li><li><h3>Benefits</h3></li><li>The <strong>Workflow List Page</strong> provides a holistic view of all workflows, their statuses, and detailed progress information. This ensures users can:</li><li>Track and manage all workflows efficiently.</li><li>Monitor progress visually and in detail.</li><li>Quickly identify completed, pending, or cancelled workflows.</li><li>This page is an essential tool for streamlining workflow operations and ensuring process transparency.</li></ul>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-20 06:09:23','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('c30fcf6b-9cd2-4536-ad93-90887f96a0ba','DOCUMENT_META_TAGS','Document Meta Tags Overview','<p>The <strong>Document Meta Tags</strong> feature allows users to manage custom metadata for documents.</p><h4><strong>Features:</strong></h4><p>✅ <strong>Add Meta Tags</strong>: Users can create new meta tags with a type, name, and editability option.<br>✅ <strong>List Meta Tags</strong>: Displays existing meta tags in a table format.<br>✅ <strong>Edit Meta Tags</strong>: Users can modify tags only if the \"Is Editable\" option is enabled.<br>✅ <strong>Delete Meta Tags</strong>: Users can remove any meta tag from the list.</p><h4><strong>Fields in the Meta Tag Table:</strong></h4><ul><li><strong>Action</strong>: Edit/Delete options (Edit allowed only if editable).</li><li><strong>Type</strong>: Defines the meta tag type (e.g., Text, Date, Number).</li><li><strong>Name</strong>: The name of the meta tag.</li><li><strong>Is Editable</strong>: Indicates whether the tag can be modified.</li></ul>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('c56481fa-3975-413b-81cb-3145a6f2fd22','OCR_CONTENT_EXTRACTOR','OCR Content Extractor','<p>The <strong>OCR Content Extractor</strong> allows users to <strong>upload files</strong> and extract text from images or scanned documents.</p><h4><strong>Features:</strong></h4><p>✅ <strong>Upload Files</strong>: Supports image and document formats (e.g., JPG, PNG, PDF).<br>✅ <strong>Extract Text</strong>: Automatically reads and converts text from uploaded files.<br>✅ <strong>Supported Languages</strong>:</p><ul><li><strong>English (ENG)</strong></li><li><strong>French (FRA)</strong></li><li><strong>Spanish (SPA)</strong></li><li><strong>Arabic (ARA)</strong></li><li><strong>Turkish (TUR)</strong></li><li><strong>Polish (POL)</strong></li><li><strong>Nepali (NEP)</strong></li><li><strong>Hindi (HIN)</strong></li></ul><p>Users can easily extract and manage text from documents in multiple languages.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('cba7325b-62e9-41b7-b6fe-4f7304eb5c70','SHARE_PERMISSIONS','Share Permissions','<h3><strong>Overview</strong></h3><p>The Share Permissions feature offers a unified interface to manage and review access permissions for both <strong>folders</strong> and <strong>documents</strong>. It enables users to share content either at the folder level (inheriting to contained documents) or at the individual document level, ensuring flexible and controlled access management. Permissions can be granted to specific users or roles, with options for download rights, email notifications, and defined access durations.</p><h3><strong>Key Features</strong></h3><h4><strong>1. Folder-Level Sharing</strong></h4><ul><li><strong>Assign by Users / Roles</strong>:<br>Two buttons—<strong>Assign By Users</strong> and <strong>Assign By Roles</strong>—allow users to share folders with specific users or user roles.</li><li><strong>Dialog for Folder Sharing</strong>:<br>A dialog opens for multi-select user or role selection, along with:<ul><li><strong>Allow Download</strong></li><li><strong>Allow Email Notification</strong></li><li><strong>Access Duration</strong> (Start Date and End Date)</li></ul></li><li><strong>Permission Inheritance</strong>:<br>When a folder is shared, all documents within the folder inherit the folder’s share permissions unless explicitly overridden at the document level.</li><li><strong>Folder Permissions List</strong>:<br>Displays all current folder share permissions, including:<ul><li><strong>User/Role Name</strong></li><li><strong>Type</strong> (User/Role)</li><li><strong>Allow Download</strong></li><li><strong>Email Notification</strong></li><li><strong>Start Date / End Date</strong></li><li><strong>Delete Option</strong> to remove permissions with confirmation dialog</li></ul></li></ul><h4><strong>2. Document-Level Sharing</strong></h4><ul><li><strong>Assign by Users / Roles</strong>:<br>Similar buttons for assigning document-specific permissions.</li><li><strong>Dialog for Document Sharing</strong>:<br>Includes:<ul><li><strong>Multi-select user/role dropdown</strong></li><li><strong>Allow Download</strong></li><li><strong>Allow Email Notification</strong></li><li><strong>Access Duration (Start and End Date)</strong></li></ul></li><li><strong>Overrides Folder Permissions</strong>:<br>If a document is also explicitly shared, its own permissions take precedence over inherited folder permissions.</li><li><strong>Document Permissions List</strong>:<br>Displays existing document-level permissions with the same columns and delete functionality as the folder section.</li></ul><h3><strong>3. Combined Permissions View</strong></h3><p>In the <strong>Share Permissions</strong> section, permissions are displayed in two separate, clearly labeled tables:</p><h4><strong>A. Folder Permissions Table</strong></h4><ul><li>Lists all users/roles with access to the folder</li><li>Shows permission metadata</li><li>Includes deletion and update capabilities</li></ul><h4><strong>B. Document Permissions Table</strong></h4><ul><li>Lists users/roles with direct access to the document (including those that override folder-level permissions)</li><li>Shows whether permissions were inherited or explicitly assigned</li><li>Same detailed view and actions as folder list</li></ul><h3><strong>4. Additional Functionalities</strong></h3><ul><li><strong>SMTP Check for Notifications</strong>:<br>If \"Allow Email Notification\" is selected, the system checks SMTP configuration. If not configured, an error prompts the user to configure SMTP first.</li><li><strong>Live Permission Update</strong>:<br>Any addition or removal of a permission instantly updates the relevant list and reflects the current state of shared access.</li><li><strong>Confirmation Dialogs</strong>:<br>Removal actions prompt confirmation to prevent accidental changes.</li></ul><h3><strong>User Interaction Flow</strong></h3><ol><li><strong>Navigate to Share Permissions</strong><br>Open the Share Permissions section in the UI for any folder or document.</li><li><strong>Assign Folder or Document Permissions</strong><ul><li>Click \"Assign By Users\" or \"Assign By Roles\"</li><li>Select entities and configure permission options</li><li>Save to apply changes</li></ul></li><li><strong>Review Permissions</strong><ul><li>View and verify entries in both the Folder and Document Permissions tables</li><li>Identify inherited vs. directly assigned permissions</li></ul></li><li><strong>Remove Permissions</strong><ul><li>Use the delete icon to remove access</li><li>Confirm via dialog</li><li>Lists update automatically</li></ul></li></ol><h3><strong>Summary</strong></h3><p>The Share Permissions feature provides a robust framework for managing who can access folders and documents. By maintaining separate but interconnected permissions for folders and documents, users have fine-grained control over access rights. Features like permission inheritance, overriding at the document level, download control, and email notifications make it a comprehensive solution for secure collaboration and content distribution.</p>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('d0e88580-71d2-4d74-b1ac-b9f34aec6818','DOCUMENT_Categories','Document Categories','<p><strong>The \"Document Categories\" page serves as a centralized hub for managing and organizing Categories, which essentially represent the departments that work with the files. It offers a hierarchical structure, allowing the creation of main Categories and subCategories.</strong></p><h4><strong>Main Components:</strong></h4><p><strong>\"Add New Document Category\" Button:</strong></p><ul><li>Allows administrators or users with appropriate permissions to create a new Category or department.</li><li>Opens a form or a pop-up window where details like the Category name and description can be entered.</li></ul><p><strong>List of Existing Categories:</strong></p><ul><li>Displays all the Categories or departments created within the system.</li><li>Each entry includes the Category name and associated action options.</li></ul><p><strong>Action Menu for Each Category:</strong></p><ul><li>Next to each Category, users will find action options that allow them to manage the Category:<ul><li><strong>Edit:</strong> Enables modification of the Category\'s details, such as the name or description.</li><li><strong>Delete:</strong> Removes the Category from the system. This action may require confirmation to prevent accidental deletions.</li></ul></li></ul><p><strong>Double Arrow Button \">>\":</strong></p><ul><li>Located next to each main Category.</li><li>When clicked, it reveals the subCategories associated with the main Category.</li><li>Allows users to view and manage subCategories in a hierarchical manner.</li></ul><h4><strong>How to Add a New Category:</strong></h4><ol><li>Click on the \"Add New Document Category\" button.</li><li>A form or pop-up window will open.</li><li>Enter the Category name and description.</li><li>Click \"Save\" or \"Add\" to add the Category to the system.</li></ol><h4><strong>How to View SubCategories:</strong></h4><ol><li>Locate the main Category in the list.</li><li>Click on the double arrow button \">>\" next to the Category name.</li><li>The associated subCategories will be displayed beneath the main Category.</li></ol>','2023-06-03 05:16:36','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 07:06:36','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('d6e392a9-b180-4c68-8566-6f289150a226','ADD_DOCUMENT','Manage document','<ul><li><strong>Allows users to upload and add a new document to the system.</strong></li><li>It includes the following fields:</li><li><strong>Upload Document:</strong> An option to upload the document file.</li><li><strong>Category:</strong> The Category under which the document is classified.</li><li><strong>Name:</strong> The name of the document.</li><li><strong>Status:</strong> The status of the document (e.g., confidential or public).</li><li><strong>Description:</strong> A detailed description or additional notes related to the document.</li><li><strong>Meta Tags:</strong> Tags or keywords associated with the document for easier searching.</li></ul>','2023-06-02 17:33:42','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 09:19:10','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('d8506639-f4ec-42d8-9939-bae893abef57','ROLES','Roles','<p><strong>The \"User Roles\" page is essential for managing and defining permissions within the CMR DMS. Roles represent predefined sets of permissions that can be assigned to users, ensuring that each user has access only to the functionalities and documents appropriate to their position and responsibilities within the organization.</strong></p><h3><strong>Main Components:</strong></h3><p><strong>\"Add Roles\" Button:</strong></p><ul><li>Allows administrators or users with appropriate permissions to create a new role in the system.</li><li>Opens a form or pop-up window where the role’s permissions and details can be defined.</li></ul><p><strong>List of Existing Roles:</strong></p><ul><li>Displays all roles created within the system in a tabular format.</li><li>Each entry includes the role name and associated action options.</li></ul><p><strong>Action Menu for Each Role:</strong></p><ul><li>Includes options for \"Edit\" and \"Delete.\"<ul><li><strong>Edit:</strong> Allows modification of the role\'s details and permissions.</li><li><strong>Delete:</strong> Removes the role from the system. This action may require confirmation to prevent accidental deletions.</li></ul></li></ul><p><strong>Role Creation Page:</strong></p><ul><li>Here, administrators can define specific permissions for each role.</li><li>Permissions can include rights such as viewing, editing, deleting, or sharing documents, managing users, defining Categories, and more.</li><li>Once permissions are set, they can be saved to create a new role or update an existing one.</li></ul><h3><strong>How to Add a New Role:</strong></h3><ol><li>Click on the \"Add Roles\" button.</li><li>A form or pop-up window will open.</li><li>Enter the role name and select the appropriate permissions from the available list.</li><li>Click \"Save\" or \"Add\" to add the role to the system with the specified permissions.</li></ol>','2023-06-03 05:18:29','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 05:52:38','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('d96ee5aa-4253-4a28-ba61-94b15b6cbfae','VERSION_HISTORY','Document versions','<p><strong>Uploading a New Version of the Document:</strong></p><p>Allows users to upload an updated or modified version of an existing document.</p><p>It includes the following fields:</p><ul><li><strong>Upload a New Version:</strong> A dedicated section for uploading a new version of the document.</li><li><strong>Restore previous version document to current version : </strong>When a user restores a previous version as the current document, the existing current document is automatically added to the document history. The restored document then becomes the active current document, ensuring effective version control and easy tracking of changes</li><li><strong>Upload Document:</strong> An option to upload the document file. Users can select the file they want to upload, and the text \"No file chosen\" will appear until a file is selected.</li><li><strong>View Document</strong>:<br>This feature provides users with the ability to preview previous versions of a document. Users can easily access and review any earlier version, allowing for better assessment and comparison before deciding to restore or make further edits.</li></ul><p><strong>How to Upload a New Version of the Document:</strong></p><ol><li>Navigate to the \"All Documents\" page.</li><li>Select the document for which you want to upload a new version.</li><li>Click on the \"Upload a New Version\" option or a similar button.</li><li>A dedicated form will open where you can select and upload the appropriate file.</li><li>After uploading the file, click \"Save\" or \"Add\" to update the document in the system with the new version.</li></ol>','2023-06-03 05:11:05','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 10:01:08','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('dd0c9840-b7c6-4a51-b78a-e674918ff7e5','NOTIFICATIONS','Notifications','<ul><li><strong>Document Shared Notification</strong>:<ul><li>Sends real-time notifications to users when a document is shared with them.</li><li>Notifications are sent via email and in-app, with details about the shared document, including name, category, and shared user.</li><li>For documents shared with external users, the recipient is notified with a secure link to access the document.</li></ul></li><li><strong>Reminder Notifications</strong>:<ul><li>Sends reminders to users for upcoming deadlines or actions related to documents (e.g., review deadlines or document expiration).</li><li>Users can configure reminder frequency and set specific reminders for important documents.</li><li>Reminders are delivered via both email and in-app notifications.</li></ul></li></ul><p>&nbsp;</p>','2023-06-03 05:28:05','4b352b37-332a-40c6-ab05-e38fcf109719','2023-08-25 19:10:29','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('df651d24-9598-4fe0-94fe-692b04789004','PROMPT_TEMPLATE','Prompt Template','<p>The <strong>Prompt Template</strong> section is designed to help users create documents using <strong>AI assistance</strong> more easily and efficiently. Instead of writing complex instructions from scratch every time, users can <strong>select or create templates</strong> that tell the AI what to do — this saves time and ensures consistent results.</p><h3> How It Works</h3><p>Each <strong>Prompt Template</strong> includes:</p><ol><li><strong>Action</strong> – This defines what the AI should do (e.g., <i>Generate Summary</i>, <i>Write Email</i>, <i>Create Report</i>).</li><li><strong>Name</strong> – A short, clear title for the template. This helps users identify it quickly.</li><li><strong>Description</strong> – A simple explanation of what the template is for and when to use it.</li><li><strong>Prompt Input</strong> – The information the user needs to provide so the AI can generate the correct output. For example: topic, customer name, product details, etc.</li></ol><h3> Why It’s Useful</h3><ul><li><strong>Easy to Use</strong>: Even users with no technical knowledge can generate documents by just filling in a few blanks.</li><li><strong>Saves Time</strong>: No need to write instructions from scratch each time.</li><li><strong>Consistent Output</strong>: Helps produce clear and structured documents every time.</li><li><strong>Flexible</strong>: Can be used for multiple use cases — from writing letters to summarizing meeting notes.</li></ul>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('ec6b2368-b8fd-4101-addf-5dec7c1d1c63','SHAREABLE_LINK','Shareable Link','<ul><li><strong>Shareable Link</strong>:<br>This feature allows users to share documents with anonymous users through a customizable link. Users have the flexibility to configure various options when creating a shareable link, including:<ul><li><strong>Start and Expiry Dates</strong>: Specify the validity period for the link, defining when it becomes active and when it expires.</li><li><strong>Password Protection</strong>: Optionally set a password to restrict access to the shared document.</li><li><strong>Download Permission</strong>: Choose whether recipients are allowed to download the document.</li></ul></li></ul><p>All options are optional, allowing users to customize the shareable link according to their preferences and requirements.</p>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 09:52:30','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('eccba93d-48bb-48f6-9784-14968d8843c8','MANAGE_USER','Manage User','<p>The User Information page is designed to collect and manage your personal details. This page is essential for setting up your user profile and ensuring you have a seamless experience using our application. Below is a brief overview of the fields you willl encounter:</p><h4><strong>Fields on the User Information Page</strong></h4><ol><li><strong>First Name</strong>:<ul><li><strong>What it is</strong>: Your given name.</li><li><strong>Importance</strong>: Helps us address you properly within the application.</li></ul></li><li><strong>Last Name</strong>:<ul><li><strong>What it is</strong>: Your family name or surname.</li><li><strong>Importance</strong>: Completes your identity and is often required for official documents.</li></ul></li><li><strong>Mobile Number</strong>:<ul><li><strong>What it is</strong>: Your phone number.</li><li><strong>Importance</strong>: Used for account recovery, notifications, and two-factor authentication. It’s optional but recommended for security purposes.</li></ul></li><li><strong>Email Address</strong>:<ul><li><strong>What it is</strong>: Your electronic mail address.</li><li><strong>Importance</strong>: Serves as your primary communication channel with us. It’s required for account verification, notifications, and password recovery.</li></ul></li><li><strong>Password</strong>:<ul><li><strong>What it is</strong>: A secret word or phrase you create to secure your account.</li><li><strong>Importance</strong>: Protects your account from unauthorized access. It must be at least 6 characters long.</li></ul></li><li><strong>Confirm Password</strong>:<ul><li><strong>What it is</strong>: A second entry of your chosen password.</li><li><strong>Importance</strong>: Ensures you’ve entered your password correctly.</li></ul></li><li><strong>Role</strong>:<ul><li><strong>What it is</strong>: Your assigned position or function within the application (e.g., Admin, User, Editor).</li><li><strong>Importance</strong>: Determines your access level and permissions within the application. This field is required to define your responsibilities and capabilities.</li></ul></li></ol><h4><strong>How to Use the Page</strong></h4><ul><li><strong>Filling Out the Form</strong>:<ul><li>Enter your information in the required fields.</li><li>Ensure that your password and confirm password entries match to avoid any errors.</li></ul></li><li><strong>Submitting Your Information</strong>:<ul><li>Once you have filled in all required fields, click the \"Submit\" button.</li><li>If any required fields are left blank or contain errors, you willl see helpful messages prompting you to correct them.</li></ul></li><li><strong>Visual Feedback</strong>:<ul><li>Fields that require your attention will be highlighted, and error messages will guide you in making the necessary corrections.</li></ul></li></ul>','2023-06-03 05:22:22','4b352b37-332a-40c6-ab05-e38fcf109719','0001-01-01 00:00:00','00000000-0000-0000-0000-000000000000',NULL,NULL,0),('ee4f69f1-1ed7-4447-87d4-c43a0b0f92e0','UPLOAD_NEW_VERSION','Upload version file','<p><strong>How to Upload a New Version of a Document:</strong></p><ol><li>Navigate to the \"All Documents\" page.</li><li>Select the document for which you want to upload a new version.</li><li>Click on the option \"Upload a New Version\" or a similar button.</li><li>A dedicated form will open, allowing you to select and upload the appropriate file.</li><li>After uploading the file, click \"Save\" or \"Add\" to update the document in the system with the new version.</li></ol>','2023-06-03 05:14:00','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 06:15:03','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('f0204d1e-59dc-43e1-83d5-86a7124692c4','GENERAL_SETTINGS','General Settings','<h4>Add Signature to PDF</h4><p>Allow users to add a digital signature to the PDF document.</p><blockquote><p><i>This option enables you to insert your signature directly into the PDF file before downloading or sharing it.</i></p></blockquote><h4> OpenAI API Key</h4><p>Enter your OpenAI API key to enable smart features like content summarization, auto-suggestions, and more.</p><blockquote><p><i>Your API key will be used to connect to OpenAI services securely.</i></p></blockquote>','2023-06-02 17:31:21','4b352b37-332a-40c6-ab05-e38fcf109719','2025-01-25 10:54:25','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('f5cecacd-f0e6-45b3-8de2-348d8ec29556','LOGIN_AUDIT_LOGS','Audit logs','<p><strong>The \"Login Audit Logs\" page serves as a centralized record for all authentication activities within CMR DMS. Here, administrators can monitor and review all login attempts, successful or failed, made by users. This provides a clear perspective on system security and user activities.</strong></p><p><strong>Main Components:</strong></p><ul><li><p><strong>Authentication Logs Table:</strong> Displays all login entries in a tabular format.</p><p>Each entry includes details such as the username, login date and time, the IP address from which the login was made, and the result (success/failure).</p></li></ul><p><strong>How to View Log Entries:</strong></p><ol><li>Navigate to the \"Login Audit Logs\" page.</li><li>Browse through the table to view all login entries.</li><li>Use the search or filter function, if available, to find specific entries.</li></ol>','2023-06-03 05:25:13','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-04 14:20:41','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('f6a1faa6-7245-4f9f-ad17-5478677bedfb','DOCUMENTS_BY_CATEGORY','Documents by Category','<p>The <strong>Homepage</strong> provides an overview of the documents within the system, showcasing statistics related to the number of documents organized by Category. It is the ideal place to quickly obtain a clear view of the volume and distribution of documents in the DMS.</p><h3>Main Components:</h3><ol><li><strong>Document Statistics</strong>:<ul><li>Displays a numerical summary of all the documents in the system, organized by Category.</li><li>Each Category is accompanied by a number indicating how many documents are in that Category.</li></ul></li><li><strong>\"Document Categories\" List</strong>:<ul><li>Shows the different document Categories available in the system, such as:<ul><li>\"Professional-Scientific_and_Education\"</li><li>\"HR Policies 2021\"</li><li>\"Professional1\"</li><li>\"Initial Complaint\"</li><li>\"HR Policies 2020\"</li><li>\"Studies_and_Strategies\"</li><li>\"Administrative_and_Financial\"</li><li>\"Approvals\"</li><li>\"Jurisdiction Commission\"</li></ul></li><li>Next to each Category, the number of documents is displayed, providing a clear view of the document distribution across Categories.</li></ul></li></ol><h3>How to interpret the statistics:</h3><ol><li>Navigate to the <strong>Statistics</strong> section on the <strong>Homepage</strong>.</li><li>View the total number of documents for each Category.<ul><li>These numbers give you an idea of the volume of documents in each Category and help identify which Categories have the most or fewest documents.</li></ul></li></ol>','2023-06-02 17:29:40','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 04:44:20','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0),('fac3d38a-5267-4b09-aea2-6682256ba777','ERROR_LOGS','Error Logs','<h4>Overview</h4><p>The <strong>ERROR_LOGS</strong> feature allows users to view the application logs generated by the backend REST API. This functionality is essential for monitoring application performance, diagnosing issues, and troubleshooting errors that may arise during API interactions.</p><h4>Features</h4><ol><li><strong>Accessing Error Logs</strong><ul><li><strong>Navigation:</strong><ul><li>Users can access the <strong>ERROR_LOGS</strong> section through the application’s administration panel or settings menu.</li></ul></li><li><strong>User Permissions:</strong><ul><li>Access to error logs may be restricted to users with specific roles, such as administrators or support staff, to maintain security and data integrity.</li></ul></li></ul></li><li><strong>Viewing Logs</strong><ul><li><strong>Log List:</strong><ul><li>The error logs are displayed in a list format, showing relevant details for each entry, including:<ul><li><strong>Timestamp:</strong> The date and time when the error occurred.</li><li><strong>Error Code:</strong> A unique code associated with the error.</li><li><strong>Error Message:</strong> A brief description of the error.</li><li><strong>Endpoint:</strong> The API endpoint that triggered the error.</li><li><strong>Request Data:</strong> The payload or parameters sent with the request (if applicable).</li><li><strong>Response Data:</strong> The response returned from the server (if applicable).</li></ul></li></ul></li><li><strong>Pagination:</strong><ul><li>Logs can be paginated to avoid overwhelming users with too much information at once, allowing users to navigate through entries easily.</li></ul></li></ul></li><li><strong>Filtering and Searching</strong><ul><li><strong>Filter Options:</strong><ul><li>Users can filter logs by various criteria, such as date range, error code, or specific endpoints, to quickly locate relevant entries.</li></ul></li><li><strong>Search Functionality:</strong><ul><li>A search bar allows users to enter keywords or phrases to find specific logs, improving the efficiency of troubleshooting.</li></ul></li></ul></li><li><strong>Log Details</strong><ul><li><strong>Expand/Collapse Feature:</strong><ul><li>Users can click on a log entry to expand and view additional details, such as:<ul><li>Full error stack trace (if available).</li><li>Contextual information regarding the request and server response.</li></ul></li></ul></li><li><strong>Export Option:</strong><ul><li>Users can export the logs in various formats (e.g., CSV, JSON) for offline analysis or reporting purposes.</li></ul></li></ul></li><li><strong>User Interaction Flow</strong><ul><li><strong>Navigating to Error Logs:</strong><ul><li>Users select the <strong>ERROR_LOGS</strong> option from the administration panel to access the log list.</li></ul></li><li><strong>Viewing and Filtering Logs:</strong><ul><li>Users can apply filters and search for specific logs to identify issues effectively.</li></ul></li><li><strong>Exploring Log Details:</strong><ul><li>Users can expand log entries to review detailed error information and troubleshoot accordingly.</li></ul></li></ul></li></ol><h3>Summary</h3><p>The <strong>ERROR_LOGS</strong> functionality provides a robust interface for users to view and manage application logs related to the backend REST API. With features such as filtering, searching, and detailed log views, users can effectively monitor application performance, diagnose errors, and troubleshoot issues, ensuring a smoother user experience and improved application reliability.</p>','2023-06-03 05:22:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-10-05 09:14:40','4b352b37-332a-40c6-ab05-e38fcf109719',NULL,NULL,0);
/*!40000 ALTER TABLE `PageHelpers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `PendingTransitions`
--

DROP TABLE IF EXISTS `PendingTransitions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `PendingTransitions` (
  `TransitionId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `FromStepId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ToStepId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TransitionName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`TransitionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `PendingTransitions`
--

LOCK TABLES `PendingTransitions` WRITE;
/*!40000 ALTER TABLE `PendingTransitions` DISABLE KEYS */;
/*!40000 ALTER TABLE `PendingTransitions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `QuarterlyReminders`
--

DROP TABLE IF EXISTS `QuarterlyReminders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `QuarterlyReminders` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ReminderId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Day` int NOT NULL,
  `Month` int NOT NULL,
  `Quarter` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_QuarterlyReminders_ReminderId` (`ReminderId`),
  CONSTRAINT `FK_QuarterlyReminders_Reminders_ReminderId` FOREIGN KEY (`ReminderId`) REFERENCES `Reminders` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `QuarterlyReminders`
--

LOCK TABLES `QuarterlyReminders` WRITE;
/*!40000 ALTER TABLE `QuarterlyReminders` DISABLE KEYS */;
/*!40000 ALTER TABLE `QuarterlyReminders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ReminderNotifications`
--

DROP TABLE IF EXISTS `ReminderNotifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ReminderNotifications` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ReminderId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Subject` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `FetchDateTime` datetime(6) NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `IsEmailNotification` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ReminderNotifications_ReminderId` (`ReminderId`),
  CONSTRAINT `FK_ReminderNotifications_Reminders_ReminderId` FOREIGN KEY (`ReminderId`) REFERENCES `Reminders` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ReminderNotifications`
--

LOCK TABLES `ReminderNotifications` WRITE;
/*!40000 ALTER TABLE `ReminderNotifications` DISABLE KEYS */;
/*!40000 ALTER TABLE `ReminderNotifications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ReminderSchedulers`
--

DROP TABLE IF EXISTS `ReminderSchedulers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ReminderSchedulers` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Duration` datetime(6) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Frequency` int DEFAULT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `IsRead` tinyint(1) NOT NULL,
  `IsEmailNotification` tinyint(1) NOT NULL,
  `Subject` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Message` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_ReminderSchedulers_DocumentId` (`DocumentId`),
  KEY `IX_ReminderSchedulers_UserId` (`UserId`),
  CONSTRAINT `FK_ReminderSchedulers_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReminderSchedulers_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ReminderSchedulers`
--

LOCK TABLES `ReminderSchedulers` WRITE;
/*!40000 ALTER TABLE `ReminderSchedulers` DISABLE KEYS */;
/*!40000 ALTER TABLE `ReminderSchedulers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ReminderUsers`
--

DROP TABLE IF EXISTS `ReminderUsers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ReminderUsers` (
  `ReminderId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`ReminderId`,`UserId`),
  KEY `IX_ReminderUsers_UserId` (`UserId`),
  CONSTRAINT `FK_ReminderUsers_Reminders_ReminderId` FOREIGN KEY (`ReminderId`) REFERENCES `Reminders` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ReminderUsers_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ReminderUsers`
--

LOCK TABLES `ReminderUsers` WRITE;
/*!40000 ALTER TABLE `ReminderUsers` DISABLE KEYS */;
/*!40000 ALTER TABLE `ReminderUsers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Reminders`
--

DROP TABLE IF EXISTS `Reminders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Reminders` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Subject` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Message` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Frequency` int DEFAULT NULL,
  `StartDate` datetime(6) NOT NULL,
  `EndDate` datetime(6) DEFAULT NULL,
  `DayOfWeek` int DEFAULT NULL,
  `IsRepeated` tinyint(1) NOT NULL,
  `IsEmailNotification` tinyint(1) NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Reminders_DocumentId` (`DocumentId`),
  CONSTRAINT `FK_Reminders_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Reminders`
--

LOCK TABLES `Reminders` WRITE;
/*!40000 ALTER TABLE `Reminders` DISABLE KEYS */;
/*!40000 ALTER TABLE `Reminders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `RoleClaims`
--

DROP TABLE IF EXISTS `RoleClaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `RoleClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `PageActionId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_RoleClaims_RoleId` (`RoleId`),
  KEY `IX_RoleClaims_PageActionId` (`PageActionId`),
  CONSTRAINT `FK_RoleClaims_PageActions_PageActionId` FOREIGN KEY (`PageActionId`) REFERENCES `PageActions` (`Id`),
  CONSTRAINT `FK_RoleClaims_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=178 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `RoleClaims`
--

LOCK TABLES `RoleClaims` WRITE;
/*!40000 ALTER TABLE `RoleClaims` DISABLE KEYS */;
INSERT INTO `RoleClaims` VALUES (38,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Email_Edit_SMTP_Setting','','8f065fb5-01c7-4dea-ab19-650392338688'),(39,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Document_Category_Edit_Category','','e67675a7-cd03-4b28-bd2f-437a813686b0'),(40,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Role_Delete_Role','','1a3346d9-3c8d-4ae0-9416-db9a157d20f2'),(41,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Role_Create_Role','','5d5e0edc-e14f-48ad-bf1d-3dfbd9ac55aa'),(42,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Role_Edit_Role','','044ceb92-87fc-41a5-93a7-ffaf096db766'),(43,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','User_Delete_User','','c9928f1f-0702-4e37-97a7-431e5c9f819c'),(44,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','User_Edit_User','','23ddf867-056f-425b-99ed-d298bbd2d80f'),(45,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','User_Create_User','','ff092131-a214-48c0-a8e3-68a8723840e1'),(46,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','User_Assign_User_Role','','e1278d04-1e53-4885-b7f3-8dd9786ee8ba'),(47,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','User_Reset_Password','','87089dd2-149a-49c4-931c-18b47e08561c'),(48,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','User_Assign_Permission','','cb980805-4de9-45b6-a12d-bb0f91d549cb'),(49,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Document_Audit_Trail_View_Document_Audit_Trail','','65dfed53-7855-46f5-ab93-3629fc68ea71'),(50,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Login_Audit_View_Login_Audit_Logs','','d886ffaa-e26f-4e27-b4e5-c3636f6422cf'),(51,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Document_Category_Create_Category','','8e82fe1f-8ccd-4cc2-b1ca-1a84dd17a5ab'),(52,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Reminder_Delete_Reminder','','ecf7dc42-fc44-4d1a-b314-d1ff71878d94'),(53,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Reminder_Create_Reminder','','6a048b38-5b3a-42b0-83fd-2c4d588d0b2f'),(54,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Dashboard_View_Dashboard','','51c88956-ea5a-4934-96ba-fd09905a1b0a'),(55,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Share_Document','','761032d2-822a-4274-ab85-3b389f5ec252'),(56,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Edit_Document','','b4fc0f33-0e9b-4b22-b357-d85125ba8d49'),(57,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Download_Document','','f8863c5a-4344-41cb-b1fa-83e223d6a7df'),(58,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Create_Document','','e99d8d8b-961c-47ad-85d8-a7b57c6a2f65'),(59,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Add_Reminder','','dcba14ed-cb99-44d4-8b4f-53d8f249ed20'),(60,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Send_Email','','faf1cb6f-9c20-4ca3-8222-32028b44e484'),(61,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Email_Delete_SMTP_Setting','','4de6055c-5f81-44d8-aee2-b966fc442263'),(62,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Email_Create_SMTP_Setting','','1a0a3737-ee82-46dc-a1b1-8bbc3aee23f6'),(63,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Reminder_Edit_Reminder','','f54926e2-3ad3-40be-8f7e-14cab77e87bd'),(64,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Document_Category_Archive_Folder','','cc5d7643-e418-492f-bbbd-409a336dbce5'),(65,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Document_Category_View_Categories','','f591c7be-4913-44f8-a74c-d2fc44dd5a3e'),(66,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Role_View_Roles','','8a90c207-7752-4277-83f6-5345ed277d7a'),(67,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','User_View_Users','','6adf6012-0101-48b2-ad54-078d2f7fe96d'),(68,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Reminder_View_Reminders','','d53f507b-c73c-435f-a4d0-69fe616b8d80'),(69,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_View_Documents','','ded2da54-9077-46b4-8d2e-db69890bed25'),(70,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Email_View_SMTP_Settings','','b13dc77a-32b9-4f48-96de-90539ba688fa'),(71,'c5d235ea-81b4-4c36-9205-2077da227c0a','Dashboard_View_Dashboard','','51c88956-ea5a-4934-96ba-fd09905a1b0a'),(72,'c5d235ea-81b4-4c36-9205-2077da227c0a','Document_Category_View_Categories','','f591c7be-4913-44f8-a74c-d2fc44dd5a3e'),(73,'c5d235ea-81b4-4c36-9205-2077da227c0a','Assigned_Documents_Create_Document','','b7d48f9a-c54c-4394-81ce-ea10aba9df87'),(74,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Create_Document','','b7d48f9a-c54c-4394-81ce-ea10aba9df87'),(75,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Delete_Comment','','197C97B0-BD33-4E2E-8C1E-BCCFD1C65FBD'),(76,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Add_Comment','','01CE3C47-67A7-4FAE-821A-494BFF0D46EF'),(77,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Add_Reminder','','DF81DECD-4282-4873-9606-AB353FCC4523'),(78,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_View_version_history','','A2148F30-0A60-420E-992A-C93B4B297DF8'),(79,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Create_Shareable_Link','','70826821-6FEE-4BF2-A64D-49CD41E7823D'),(80,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Create_Delete_Comment','','B470AE88-E223-45DE-8720-3D4467A5F702'),(81,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Create_Restore_version','','B0081952-68C8-4C89-83B1-3D5C97941C19'),(82,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Create_Add_Comment','','616254DE-2EA2-46E3-B7C3-4596D6180144'),(83,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_View_version_history','','63B2165E-CB49-4F10-B504-082F618B76F0'),(84,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Create_Shareable_Link','','6E52219E-0B7F-4E57-B7B3-DB688DE17AF0'),(85,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Upload_New_version','','74BDCA6C-DDF5-49A6-9719-F364AB7BEE11'),(86,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Document_Status_Manage_Document_Status','','77B6DC42-AECE-4FDB-8203-06B61C2DDDB0'),(87,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Storage_Settings_Manage_Storage_Settings','','FC1D752F-005B-4CAE-9303-B7557EEE7461'),(88,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Company_Profile_Manage_Company_Settings','','61C15E32-28A9-4DCE-8CDE-5CA8325F5F04'),(89,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Page_Helper_Manage_Page_Helper','','2A9169A8-A46E-4D61-94FC-AA8A9ADC459A'),(90,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Error_Logs_View_Error_Logs','','D21077FE-3441-4AD2-BD22-BD13233EE5B2'),(91,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Deep_Search_Deep_Search','','42794D41-EF8A-4F8E-B0AD-1FCEB50B3E2E'),(92,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Deep_Search_Add_Document_Index','','ECA884BB-CCB8-4723-A137-D73F988AE300'),(93,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Deep_Search_Remove_Document_Index','','A73CCEF3-DA47-49E8-9944-EBA0F18B2E40'),(101,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','assigned_documents_add_signature','','2e159fd9-df31-400f-9dde-88bdc34bf42f'),(102,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Workflow_Logs_Workflow_Logs','','95322722-6362-41ac-8165-9c64076399fb'),(103,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Workflows_Workflows','','1eb6b640-de77-4d96-8487-222367b1233d'),(104,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Current_Workflow_Current_Workflow','','5d4859df-2354-4e77-b3c2-f9eb85f7836d'),(105,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Workflow_Settings_Edit_Workflow_Settings','','c45ee6ab-20a8-44b2-ae88-8fcbf4fbd54d'),(106,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Workflow_Settings_Add_Workflow_Settings','','f2bb13d3-c376-42ab-b804-952dfd8ce164'),(107,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Workflow_Settings_View_Workflow_Settings','','f7dc4930-c87f-429d-be36-6b9817adf1e8'),(108,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Workflow_Settings_Delete_Workflow_Settings','','db16125a-e16e-498f-980f-7f8f397dd920'),(109,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Add_Signature','','2e159fd9-df31-400f-9dde-88bdc34bf42f'),(110,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Start_Workflow','','3729cbe2-302d-4096-934e-b4cad63711e3'),(111,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Start_Workflow','','e37e63ee-3aa4-4521-b913-95d48b8c831a'),(114,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Add_Signature','','07767bf0-ca74-423b-9ad2-0e2a0bd1087b'),(117,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Archive_Documents_View_Documents','','7999014d-dfcd-4f2c-b6d8-cf66571d1fb7'),(122,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','File_Request_Add_File_Request','','97ab4ba3-be33-4d8b-9363-86cf013f592c'),(123,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','File_Request_Edit_File_Request','','9eac28ed-e662-4c9c-8bf9-8e14d7568afc'),(124,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','File_Request_Delete_File_Request','','bc56d098-f652-430c-be0d-210090a298cb'),(125,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','File_Request_View_File_Request','','38862aeb-f2d6-4256-b506-3298f12d0f03'),(126,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','File_Request_Approved_File_Request_Documents','','5c7f8415-59ac-4ae7-a622-a5729aaac8ab'),(127,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','File_Request_Rejected_File_Request_Documents','','1c0a486d-53a9-4239-97ae-233366a63646'),(128,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Archive_Document','','930af13c-158e-493a-ba7c-6406587b5286'),(129,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Archive_Document','','670fc04b-7367-4984-8aeb-037705d7bfc8'),(130,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Archive_Documents_Create_Shareable_Link','','5dbdba2a-17f7-48f9-989c-52322fd24d8c'),(131,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Archive_Documents_Download_Document','','0a74c66d-10ec-43a8-ab35-7b45ce0d6092'),(132,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Archive_Documents_Restore_Document','','00e99d70-a221-47e6-b56f-cc19e154b6c6'),(133,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Archive_Documents_Delete_Document','','5b5e4d58-cdfc-4e4b-8083-9b497230af4b'),(134,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Archive_Documents_View_version_history','','8105bd15-f1e3-435b-be45-38cf8dea8800'),(135,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Archive_Documents_Send_Email','','617c390b-99c7-4a22-bd16-154aecc63169'),(136,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Bulk_Document_Uploads_Bulk_Document_Uploads','','35746e9b-7bb3-4272-b0f3-5abde75fe6cd'),(137,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Clients_View_Clients','','82e66203-b6d6-440e-8c9b-733163b574a1'),(138,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Clients_Add_Clients','','c8e790f2-6fb6-412a-a2fb-45dc579cae43'),(139,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Clients_Edit_Clients','','c5b79883-882f-4e1f-bbbe-f6ed055484ca'),(140,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Clients_Delete_Clients','','2c5ac3ce-a7e8-4feb-80b7-8fe10d412631'),(141,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Allow_To_See_File_Request','','8a2e6320-2aa2-496c-9e72-0cd7be0140df'),(142,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Remove_Share_Folder','','44195689-7037-4bce-9e5f-b7de96c4fba8'),(143,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Remove_Share_Folder','','d40e679d-a08a-4dde-bca8-e75ebfdbd817'),(144,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Remove_Share_Document','','d2ae9a3b-a491-4a07-b155-6401ef11712a'),(145,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Remove_Share_Document','','aa12fe9c-3388-429c-90f8-070fb01d5868'),(146,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Document_Meta_Tags_View_Document_Meta_Tags','','5f3963ab-ceb2-443e-94b6-9ef0752a2d2e'),(147,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Document_Meta_Tags_Edit_Document_Meta_Tags','','4cd7059f-53b5-4ec2-be6b-78cdab96eb70'),(148,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Document_Meta_Tags_Add_Document_Meta_Tags','','80071200-5e95-4b91-9334-c1d80a2c9b46'),(149,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Document_Meta_Tags_Delete_Document_Meta_Tags','','35b754c5-0e48-4788-a71c-4de13a8fe449'),(150,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Folders_Share_Folder','','ffa7871f-de6f-4265-9548-4177e2bf5dfe'),(151,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Folders_Archive_Folder','','b1375795-6655-47fc-be67-5569a2fe2517'),(152,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Folders_Share_Folder','','c2b9abde-bf77-4887-b18e-4a84d24699de'),(153,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Folders_Archive_Folder','','c3768706-4767-472e-9bbf-0c515bec6ff4'),(154,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Archive_Folders_Restore_Folder','','df42ffb1-f957-4c63-b07a-c57b1f915ff2'),(155,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Archive_Folders_Delete_Category','','724b0bb9-d15f-4ad6-bf43-a76b1a393bf6'),(156,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','OCR_Content_Extractor_OCR_Content_Extractor','','3b11f492-781a-4640-86ec-4382bc46d7d0'),(157,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Allow_File_Extensions_Add_Allow_File_Extensions','','bcd3bbf9-fff5-4ece-b82e-35b130406b25'),(158,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Allow_File_Extensions_Edit_Allow_File_Extensions','','7b90dcef-ec68-4c75-ab24-c1bf2c365b09'),(159,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Allow_File_Extensions_View_Allow_File_Extensions','','4b60217a-a360-40d2-8641-d7d336a1a1e4'),(160,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Allow_File_Extensions_Delete_Allow_File_Extensions','','2f4259e3-6f34-4f8d-9189-b71e5efeb8e1'),(161,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','All_Documents_Get_Document_Summary','','6fd5cbb9-d1b5-4915-9fa9-ab036a5b26f6'),(162,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Documents_Get_Document_Summary','','fb546f57-ce8a-4a58-8c9e-43955c3ea392'),(163,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','AI_Documents_AI_Document_Generator','','39836641-143c-4e37-bc18-12af4eb95f01'),(164,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','AI_Documents_View_Prompt_Templates','','14f96237-e4ad-4cdb-af33-dc0f98abe121'),(165,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','General_Settings_General_Settings','','1d831629-f1b9-4c47-b70d-97893908fb46'),(166,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','AI_Documents_Add_Prompt_Templates','','37653d9f-73d2-459f-ba17-e678944c0703'),(167,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','AI_Documents_Edit_Prompt_Templates','','ed9ec981-4e36-436d-962a-44c2f347213d'),(168,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','AI_Documents_Delete_Prompt_Templates','','5ea98fd6-f86f-462b-ba4a-c2a7cadcf122'),(169,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','AI_Documents_View_AI_Document_Generator','','cfde1c32-3f74-47e7-b0c8-f7061acac39b'),(170,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','AI_Documents_Delete_AI_Document_Generator','','3c7ea13e-e6be-4035-90cd-9fdf793ce89f'),(171,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Document_Category_Share_Folder','','8c98e9a6-8edc-4e4e-8714-37752437f10d'),(172,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Archive_Retention_Period_Manage_Archive_Retention_Period','','d5e8bac8-c173-459b-be1d-394b2b46f2d9'),(173,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Edit_Document','','d1f39f95-d550-474b-96fa-99b097b34c1b'),(174,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Restore_version','','764568df-718a-4a10-a6b5-a11523b22c19'),(175,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Send_Email','','e945d35f-d3f6-46e7-9723-a62f89b2022e'),(176,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Share_Document','','723e56ce-a020-452f-8773-2f27e2725f1b'),(177,'fedeac7a-a665-40a4-af02-f47ec4b7aff5','Assigned_Upload_New_version','','dcf6f998-7276-4a1c-aba1-8513f2eaebad');
/*!40000 ALTER TABLE `RoleClaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Roles`
--

DROP TABLE IF EXISTS `Roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Roles` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Roles`
--

LOCK TABLES `Roles` WRITE;
/*!40000 ALTER TABLE `Roles` DISABLE KEYS */;
INSERT INTO `Roles` VALUES ('c5d235ea-81b4-4c36-9205-2077da227c0a',0,'Employee','Employee','47432aba-cc42-4113-a49d-cb8548e185b2'),('fedeac7a-a665-40a4-af02-f47ec4b7aff5',0,'Super Admin','Super Admin','870b5668-b97a-4406-bead-09022612568c');
/*!40000 ALTER TABLE `Roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Screens`
--

DROP TABLE IF EXISTS `Screens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Screens` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `OrderNo` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Screens`
--

LOCK TABLES `Screens` WRITE;
/*!40000 ALTER TABLE `Screens` DISABLE KEYS */;
INSERT INTO `Screens` VALUES ('090ea443-01c7-4638-a194-ad3416a5ea7a','Role','2021-12-22 16:17:44','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 16:17:44','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,23),('2396f81c-f8b5-49ac-88d1-94ed57333f49','Document Audit Trail','2021-12-22 16:17:39','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 16:17:39','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,17),('2502e117-1752-4c6f-8fdb-cb32cb4c1e59','Archive Folders','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,19),('28e6dbe7-1d17-4875-b96f-8757870d20af','General Settings','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,26),('2e3c07a4-fcac-4303-ae47-0d0f796403c9','Email','2021-12-22 16:18:01','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 16:18:01','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,25),('309f72e8-6e93-43fa-9203-808b030a33f7','Workflow Settings','2024-12-23 06:58:24','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:58:24','00000000-0000-0000-0000-000000000000',NULL,NULL,0,14),('324bdc51-d71f-4f80-9f28-a30e8aae4009','User','2021-12-22 16:17:49','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 16:17:49','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,24),('33c5442e-2844-497c-af00-9cda5a6fc826','Workflow Logs','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,15),('42338E4F-05E0-48D1-862A-D977C39D02DF','Error Logs','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000',NULL,NULL,0,33),('42e44f15-8e33-423a-ad7f-17edc23d6dd3','Dashboard','2021-12-22 16:17:16','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 16:17:16','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,1),('4513EAE1-373A-4734-928C-8943C3F070BB','Document Status','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000',NULL,NULL,0,21),('479b5173-369e-487a-817d-651d77782590','AI Documents','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,8),('5a5f7cf8-21a6-434a-9330-db91b17d867c','Document Category','2021-12-22 16:17:33','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 16:17:33','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,6),('669C82F1-0DE0-459C-B62A-83A9614259E4','Company Profile','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000',NULL,NULL,0,30),('73DF018E-77B1-42FB-B8D7-D7A09836E453','Page Helper','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000',NULL,NULL,0,28),('8130904f-4e5b-47d4-8acf-c9cff36d48a4','Archive Documents','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,18),('97ff6eb0-39b3-4ddd-acf1-43205d5a9bb3','Reminder','2021-12-22 16:17:53','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 16:17:53','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,16),('9e58a819-05ce-4226-ad70-31d2897f94be','Archive Retention Period','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,20),('a51a1261-aa38-4c07-a513-38c8e619d141','Document Meta Tags','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,31),('a676aa9b-8ccb-4608-8e21-2cdd9a37fd99','Current Workflow','2024-12-23 06:59:00','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:00','00000000-0000-0000-0000-000000000000',NULL,NULL,0,12),('b6058070-8cca-4bc0-a88a-5596cbd63683','All Folders','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,5),('c6d248e0-f702-4334-b0b8-0c403a82715f','Allow File Extensions','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,29),('D6B63A2E-F7DB-4F83-B5DD-96A000D9A789','Deep Search','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000',NULL,NULL,0,7),('da660d2d-450b-4825-9489-d87ef94ad6ff','Assigned Folders','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,3),('dc075340-895b-41c0-93da-1767f097a64b','Clients','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,22),('eb342fd3-836f-437c-9b6b-b7e4188fb49c','Bulk Document Uploads','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,10),('eddf9e8e-0c70-4cde-b5f9-117a879747d6','All Documents','2021-12-22 16:17:24','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 16:17:24','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,4),('efe7d58a-1c33-4f2a-9b7b-3e1dff6b5664','Workflows','2024-12-23 06:58:44','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:58:44','00000000-0000-0000-0000-000000000000',NULL,NULL,0,13),('f042bbee-d15f-40fb-b79a-8368f2c2e287','Login Audit','2021-12-22 16:17:57','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-22 16:17:57','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,32),('f4a940bd-8f81-4b5c-abd1-0ddb8083c4da','OCR Content Extractor','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,9),('FC1D752F-005B-4CAE-9303-B7557EEE7461','Storage Settings','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000','2024-10-22 16:18:01','00000000-0000-0000-0000-000000000000',NULL,NULL,0,27),('fc97dc8f-b4da-46b1-a179-ab206d8b7efd','Assigned Documents','2021-12-24 10:15:02','4b352b37-332a-40c6-ab05-e38fcf109719','2021-12-24 10:15:02','00000000-0000-0000-0000-000000000000',NULL,'00000000-0000-0000-0000-000000000000',0,2),('ffee08a0-35e0-485a-a335-e455fb59e344','File Request','2024-12-23 06:59:22','4b352b37-332a-40c6-ab05-e38fcf109719','2024-12-23 06:59:22','00000000-0000-0000-0000-000000000000',NULL,NULL,0,11);
/*!40000 ALTER TABLE `Screens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SendEmails`
--

DROP TABLE IF EXISTS `SendEmails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SendEmails` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Subject` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Message` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `FromEmail` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsSend` tinyint(1) NOT NULL,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `FromName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ToName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_SendEmails_DocumentId` (`DocumentId`),
  CONSTRAINT `FK_SendEmails_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SendEmails`
--

LOCK TABLES `SendEmails` WRITE;
/*!40000 ALTER TABLE `SendEmails` DISABLE KEYS */;
/*!40000 ALTER TABLE `SendEmails` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `StorageSettings`
--

DROP TABLE IF EXISTS `StorageSettings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `StorageSettings` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `JsonValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsDefault` tinyint(1) NOT NULL,
  `EnableEncryption` tinyint(1) NOT NULL,
  `StorageType` int NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `StorageSettings`
--

LOCK TABLES `StorageSettings` WRITE;
/*!40000 ALTER TABLE `StorageSettings` DISABLE KEYS */;
INSERT INTO `StorageSettings` VALUES ('06890479-e463-4f40-a9a0-080c03e3f7a4','Local Storage',NULL,1,0,2,'2024-09-30 14:16:39','00000000-0000-0000-0000-000000000000','2024-09-30 14:16:39','00000000-0000-0000-0000-000000000000',NULL,NULL,0);
/*!40000 ALTER TABLE `StorageSettings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserClaims`
--

DROP TABLE IF EXISTS `UserClaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `PageActionId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_UserClaims_UserId` (`UserId`),
  KEY `IX_UserClaims_PageActionId` (`PageActionId`),
  CONSTRAINT `FK_UserClaims_PageActions_PageActionId` FOREIGN KEY (`PageActionId`) REFERENCES `PageActions` (`Id`),
  CONSTRAINT `FK_UserClaims_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserClaims`
--

LOCK TABLES `UserClaims` WRITE;
/*!40000 ALTER TABLE `UserClaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserClaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserLogins`
--

DROP TABLE IF EXISTS `UserLogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserLogins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_UserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_UserLogins_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserLogins`
--

LOCK TABLES `UserLogins` WRITE;
/*!40000 ALTER TABLE `UserLogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserLogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserNotifications`
--

DROP TABLE IF EXISTS `UserNotifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserNotifications` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Message` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsRead` tinyint(1) NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `NotificationsType` int NOT NULL DEFAULT '0',
  `FileRequestDocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `WorkflowInstanceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CategoryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_UserNotifications_DocumentId` (`DocumentId`),
  KEY `IX_UserNotifications_UserId` (`UserId`),
  KEY `IX_UserNotifications_FileRequestDocumentId` (`FileRequestDocumentId`),
  KEY `IX_UserNotifications_WorkflowInstanceId` (`WorkflowInstanceId`),
  KEY `IX_UserNotifications_CategoryId` (`CategoryId`),
  CONSTRAINT `FK_UserNotifications_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`Id`),
  CONSTRAINT `FK_UserNotifications_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_UserNotifications_FileRequestDocuments_FileRequestDocumentId` FOREIGN KEY (`FileRequestDocumentId`) REFERENCES `FileRequestDocuments` (`Id`),
  CONSTRAINT `FK_UserNotifications_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`),
  CONSTRAINT `FK_UserNotifications_WorkflowInstances_WorkflowInstanceId` FOREIGN KEY (`WorkflowInstanceId`) REFERENCES `WorkflowInstances` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserNotifications`
--

LOCK TABLES `UserNotifications` WRITE;
/*!40000 ALTER TABLE `UserNotifications` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserNotifications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserOpenaiMsgs`
--

DROP TABLE IF EXISTS `UserOpenaiMsgs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserOpenaiMsgs` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `PromptInput` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Language` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `MaximumLength` int NOT NULL,
  `Creativity` decimal(65,30) NOT NULL,
  `ToneOfVoice` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `SelectedModel` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `AiResponse` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_UserOpenaiMsgs_CreatedBy` (`CreatedBy`),
  CONSTRAINT `FK_UserOpenaiMsgs_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserOpenaiMsgs`
--

LOCK TABLES `UserOpenaiMsgs` WRITE;
/*!40000 ALTER TABLE `UserOpenaiMsgs` DISABLE KEYS */;
INSERT INTO `UserOpenaiMsgs` VALUES ('08de2d94-b2cb-4030-8faa-9d241fd5eea3','','generate a page about me','en-US',1000,0.250000000000000000000000000000,'Professional','gemini-2.5-flash',NULL,'2025-11-27 09:09:41','4b352b37-332a-40c6-ab05-e38fcf109719','0001-01-01 00:00:00','00000000-0000-0000-0000-000000000000',NULL,NULL,0),('08de2da0-11a8-4574-89fa-32eafbf6ee75','','hello','en-US',1000,0.250000000000000000000000000000,'Professional','gpt-3.5-turbo',NULL,'2025-11-27 10:31:05','4b352b37-332a-40c6-ab05-e38fcf109719','0001-01-01 00:00:00','00000000-0000-0000-0000-000000000000',NULL,NULL,0);
/*!40000 ALTER TABLE `UserOpenaiMsgs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserRoles`
--

DROP TABLE IF EXISTS `UserRoles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserRoles` (
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RoleId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_UserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_UserRoles_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_UserRoles_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserRoles`
--

LOCK TABLES `UserRoles` WRITE;
/*!40000 ALTER TABLE `UserRoles` DISABLE KEYS */;
INSERT INTO `UserRoles` VALUES ('1a5cf5b9-ead8-495c-8719-2d8be776f452','c5d235ea-81b4-4c36-9205-2077da227c0a'),('4b352b37-332a-40c6-ab05-e38fcf109719','fedeac7a-a665-40a4-af02-f47ec4b7aff5');
/*!40000 ALTER TABLE `UserRoles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserTokens`
--

DROP TABLE IF EXISTS `UserTokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserTokens` (
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_UserTokens_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserTokens`
--

LOCK TABLES `UserTokens` WRITE;
/*!40000 ALTER TABLE `UserTokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserTokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Users`
--

DROP TABLE IF EXISTS `Users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Users` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `FirstName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `LastName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsDeleted` tinyint(1) NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  `ClientId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ClientSecretHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsSuperAdmin` tinyint(1) NOT NULL DEFAULT '0',
  `IsSystemUser` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Users`
--

LOCK TABLES `Users` WRITE;
/*!40000 ALTER TABLE `Users` DISABLE KEYS */;
INSERT INTO `Users` VALUES ('1a5cf5b9-ead8-495c-8719-2d8be776f452','Shirley','Heitzman',0,'employee@gmail.com','EMPLOYEE@GMAIL.COM','employee@gmail.com','EMPLOYEE@GMAIL.COM',0,'AQAAAAIAAYagAAAAEGeQEWQQisCZ8ol8whiLjFhZ6Duel55DAFjc5aq/DOrgAEpaCBpBdqOlPa9usZr6ag==','PYFKBYTOTOWVGER25SQ67TIYXREGAYHM','366ab087-be25-4577-bc8c-0ca7396dab1f','9904750722',0,0,NULL,1,0,NULL,NULL,0,0),('4b352b37-332a-40c6-ab05-e38fcf109719','David','Parnell',0,'admin@gmail.com','ADMIN@GMAIL.COM','admin@gmail.com','ADMIN@GMAIL.COM',0,'AQAAAAIAAYagAAAAECZl0yALjAkSFNt0wZbCdrtUzjmIzdkdutwSeGbDb2DqRLTefPg3MSq4ce2I9UOx2A==','F3E3XL33N3UZ76JJ7IF3VYHAFUBA6R3S','87ac9c25-d926-4d3d-9754-6ae0b8daf60e','1234567890',0,0,NULL,1,0,NULL,NULL,1,0),('6e8b6f4a-9c7d-47b6-a476-b94ac78e2db1','System','User',0,'systemuser@gmail.com','SYSTEMUSER@GMAIL.COM','systemuser@gmail.com','SYSTEMUSER@GMAIL.COM',0,'AQAAAAEAACcQAAAAEM60FYHL5RMKNeB+CxCOI41EC8Vsr1B3Dyrrr2BOtZrxz6doL8o6Tv/tYGDRk20t1A==','f3bda79c-c5e4-47ad-bdf5-d8d01c983de2','b8f676e1-3e2a-4c3c-b58d-772a9c47e4f9','1234567890',0,0,NULL,1,0,NULL,NULL,0,1);
/*!40000 ALTER TABLE `Users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `WorkflowInstanceEmailSenders`
--

DROP TABLE IF EXISTS `WorkflowInstanceEmailSenders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WorkflowInstanceEmailSenders` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `WorkflowStepInstanceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `WorkflowTransitionId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_WorkflowInstanceEmailSenders_WorkflowStepInstanceId` (`WorkflowStepInstanceId`),
  KEY `IX_WorkflowInstanceEmailSenders_WorkflowTransitionId` (`WorkflowTransitionId`),
  CONSTRAINT `FK_WorkflowInstanceEmailSenders_WorkflowStepInstances_WorkflowS~` FOREIGN KEY (`WorkflowStepInstanceId`) REFERENCES `WorkflowStepInstances` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_WorkflowInstanceEmailSenders_WorkflowTransitions_WorkflowTra~` FOREIGN KEY (`WorkflowTransitionId`) REFERENCES `WorkflowTransitions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `WorkflowInstanceEmailSenders`
--

LOCK TABLES `WorkflowInstanceEmailSenders` WRITE;
/*!40000 ALTER TABLE `WorkflowInstanceEmailSenders` DISABLE KEYS */;
/*!40000 ALTER TABLE `WorkflowInstanceEmailSenders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `WorkflowInstances`
--

DROP TABLE IF EXISTS `WorkflowInstances`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WorkflowInstances` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `WorkflowId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `InitiatedId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_WorkflowInstances_DocumentId` (`DocumentId`),
  KEY `IX_WorkflowInstances_InitiatedId` (`InitiatedId`),
  KEY `IX_WorkflowInstances_WorkflowId` (`WorkflowId`),
  CONSTRAINT `FK_WorkflowInstances_Documents_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `Documents` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_WorkflowInstances_Users_InitiatedId` FOREIGN KEY (`InitiatedId`) REFERENCES `Users` (`Id`),
  CONSTRAINT `FK_WorkflowInstances_Workflows_WorkflowId` FOREIGN KEY (`WorkflowId`) REFERENCES `Workflows` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `WorkflowInstances`
--

LOCK TABLES `WorkflowInstances` WRITE;
/*!40000 ALTER TABLE `WorkflowInstances` DISABLE KEYS */;
/*!40000 ALTER TABLE `WorkflowInstances` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `WorkflowStepInstances`
--

DROP TABLE IF EXISTS `WorkflowStepInstances`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WorkflowStepInstances` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `WorkflowInstanceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `StepId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Status` int NOT NULL,
  `CompletedAt` datetime(6) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `Comment` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_WorkflowStepInstances_StepId` (`StepId`),
  KEY `IX_WorkflowStepInstances_UserId` (`UserId`),
  KEY `IX_WorkflowStepInstances_WorkflowInstanceId` (`WorkflowInstanceId`),
  CONSTRAINT `FK_WorkflowStepInstances_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`),
  CONSTRAINT `FK_WorkflowStepInstances_WorkflowInstances_WorkflowInstanceId` FOREIGN KEY (`WorkflowInstanceId`) REFERENCES `WorkflowInstances` (`Id`),
  CONSTRAINT `FK_WorkflowStepInstances_WorkflowSteps_StepId` FOREIGN KEY (`StepId`) REFERENCES `WorkflowSteps` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `WorkflowStepInstances`
--

LOCK TABLES `WorkflowStepInstances` WRITE;
/*!40000 ALTER TABLE `WorkflowStepInstances` DISABLE KEYS */;
/*!40000 ALTER TABLE `WorkflowStepInstances` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `WorkflowSteps`
--

DROP TABLE IF EXISTS `WorkflowSteps`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WorkflowSteps` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `WorkflowId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `StepName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `IsFinal` tinyint(1) NOT NULL,
  `OrderNo` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_WorkflowSteps_WorkflowId` (`WorkflowId`),
  CONSTRAINT `FK_WorkflowSteps_Workflows_WorkflowId` FOREIGN KEY (`WorkflowId`) REFERENCES `Workflows` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `WorkflowSteps`
--

LOCK TABLES `WorkflowSteps` WRITE;
/*!40000 ALTER TABLE `WorkflowSteps` DISABLE KEYS */;
/*!40000 ALTER TABLE `WorkflowSteps` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `WorkflowTransitionInstances`
--

DROP TABLE IF EXISTS `WorkflowTransitionInstances`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WorkflowTransitionInstances` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Status` int NOT NULL,
  `WorkflowTransitionId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `WorkflowInstanceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `Comment` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `PerformById` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_WorkflowTransitionInstances_PerformById` (`PerformById`),
  KEY `IX_WorkflowTransitionInstances_WorkflowInstanceId` (`WorkflowInstanceId`),
  KEY `IX_WorkflowTransitionInstances_WorkflowTransitionId` (`WorkflowTransitionId`),
  CONSTRAINT `FK_WorkflowTransitionInstances_Users_PerformById` FOREIGN KEY (`PerformById`) REFERENCES `Users` (`Id`),
  CONSTRAINT `FK_WorkflowTransitionInstances_WorkflowInstances_WorkflowInstan~` FOREIGN KEY (`WorkflowInstanceId`) REFERENCES `WorkflowInstances` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_WorkflowTransitionInstances_WorkflowTransitions_WorkflowTran~` FOREIGN KEY (`WorkflowTransitionId`) REFERENCES `WorkflowTransitions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `WorkflowTransitionInstances`
--

LOCK TABLES `WorkflowTransitionInstances` WRITE;
/*!40000 ALTER TABLE `WorkflowTransitionInstances` DISABLE KEYS */;
/*!40000 ALTER TABLE `WorkflowTransitionInstances` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `WorkflowTransitionRoles`
--

DROP TABLE IF EXISTS `WorkflowTransitionRoles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WorkflowTransitionRoles` (
  `WorkflowTransitionId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RoleId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`WorkflowTransitionId`,`RoleId`),
  KEY `IX_WorkflowTransitionRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_WorkflowTransitionRoles_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_WorkflowTransitionRoles_WorkflowTransitions_WorkflowTransiti~` FOREIGN KEY (`WorkflowTransitionId`) REFERENCES `WorkflowTransitions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `WorkflowTransitionRoles`
--

LOCK TABLES `WorkflowTransitionRoles` WRITE;
/*!40000 ALTER TABLE `WorkflowTransitionRoles` DISABLE KEYS */;
/*!40000 ALTER TABLE `WorkflowTransitionRoles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `WorkflowTransitionUsers`
--

DROP TABLE IF EXISTS `WorkflowTransitionUsers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WorkflowTransitionUsers` (
  `WorkflowTransitionId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`WorkflowTransitionId`,`UserId`),
  KEY `IX_WorkflowTransitionUsers_UserId` (`UserId`),
  CONSTRAINT `FK_WorkflowTransitionUsers_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_WorkflowTransitionUsers_WorkflowTransitions_WorkflowTransiti~` FOREIGN KEY (`WorkflowTransitionId`) REFERENCES `WorkflowTransitions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `WorkflowTransitionUsers`
--

LOCK TABLES `WorkflowTransitionUsers` WRITE;
/*!40000 ALTER TABLE `WorkflowTransitionUsers` DISABLE KEYS */;
/*!40000 ALTER TABLE `WorkflowTransitionUsers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `WorkflowTransitions`
--

DROP TABLE IF EXISTS `WorkflowTransitions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WorkflowTransitions` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `WorkflowId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `FromStepId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ToStepId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Condition` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `IsFirstTransaction` tinyint(1) NOT NULL,
  `Days` int DEFAULT NULL,
  `Hours` int DEFAULT NULL,
  `Minutes` int DEFAULT NULL,
  `IsUploadDocumentVersion` tinyint(1) NOT NULL,
  `Color` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsSignatureRequired` tinyint(1) NOT NULL DEFAULT '0',
  `OrderNo` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_WorkflowTransitions_FromStepId` (`FromStepId`),
  KEY `IX_WorkflowTransitions_ToStepId` (`ToStepId`),
  KEY `IX_WorkflowTransitions_WorkflowId` (`WorkflowId`),
  CONSTRAINT `FK_WorkflowTransitions_Workflows_WorkflowId` FOREIGN KEY (`WorkflowId`) REFERENCES `Workflows` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_WorkflowTransitions_WorkflowSteps_FromStepId` FOREIGN KEY (`FromStepId`) REFERENCES `WorkflowSteps` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_WorkflowTransitions_WorkflowSteps_ToStepId` FOREIGN KEY (`ToStepId`) REFERENCES `WorkflowSteps` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `WorkflowTransitions`
--

LOCK TABLES `WorkflowTransitions` WRITE;
/*!40000 ALTER TABLE `WorkflowTransitions` DISABLE KEYS */;
/*!40000 ALTER TABLE `WorkflowTransitions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Workflows`
--

DROP TABLE IF EXISTS `Workflows`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Workflows` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `IsWorkflowSetup` tinyint(1) NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModifiedDate` datetime NOT NULL,
  `ModifiedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeletedDate` datetime DEFAULT NULL,
  `DeletedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Workflows_UserId` (`UserId`),
  CONSTRAINT `FK_Workflows_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Workflows`
--

LOCK TABLES `Workflows` WRITE;
/*!40000 ALTER TABLE `Workflows` DISABLE KEYS */;
/*!40000 ALTER TABLE `Workflows` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20211225124149_Initial','8.0.13'),('20211225124206_Initial_SQL_Data','8.0.13'),('20220620111304_Version_V3','8.0.13'),('20241009065257_Version_V31_MySql','8.0.13'),('20241009065309_Version_V31_MySql_Data','8.0.13'),('20250214070531_Version_V5_MySql','8.0.13'),('20250214070546_Version_V5_MySql_Data','8.0.13'),('20250301055942_Version_V51_MySql','8.0.13'),('20250308083401_Added_TotalChunk_INTO_DocumentChunk_Table','8.0.13'),('20250402145807_Version_V6_MySql','8.0.13'),('20250402145821_Version_V6_MySql_Data','8.0.13'),('20250505113315_Version_V7_MySql','8.0.13'),('20250505113344_Version_V7_MySql_Data','8.0.13'),('20250623051735_Version_V8_MySql','8.0.13'),('20250623051755_Version_V8_MySql_Data','8.0.13'),('20250923052831_Added_PAGEACTION_Table','8.0.13'),('20250923102444_Added_PageActionId_Into_UserClaim_RoleClaim','8.0.13'),('20250923102948_Allow_ScreenId_Null','8.0.13'),('20250923103030_Entry_Into_PageAction','8.0.13'),('20250923103109_Update_PageAction_Code','8.0.13'),('20250923103355_Added_Role_And_User_Claim_Remove_Screen_Operation','8.0.13'),('20250923103459_REMOVE_OPERATION_OPERATIONSCREEN_TABLES','8.0.13'),('20250923103531_Added_Update_Code_Same_ClaimName','8.0.13'),('20250923103621_UPDATE_CLAIM_VALUE','8.0.13'),('20250923103703_Remove_PageName_From_PageAction','8.0.13'),('20250923104037_Added_GeminiAPIKey_CompanyProfile','8.0.13'),('20250924100646_Remove_Duplicate_PageActions','8.0.13');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-11-27 15:41:38
