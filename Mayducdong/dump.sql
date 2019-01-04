-- MySQL dump 10.13  Distrib 5.6.37, for Win32 (AMD64)
--
-- Host: localhost    Database: mmslocalmayducdong
-- ------------------------------------------------------
-- Server version	5.6.37

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `mayducdong_qtsx`
--

DROP TABLE IF EXISTS `mayducdong_qtsx`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mayducdong_qtsx` (
  `msnv` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `L` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `W` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `H` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Hole` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `ID_lohang` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `date_start` datetime DEFAULT NULL,
  `date_finish` datetime DEFAULT NULL,
  `So_luong_thanh` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `So_luong_lo` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Trang_thai_start` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Trang_thai_stop` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mayducdong_qtsx`
--

LOCK TABLES `mayducdong_qtsx` WRITE;
/*!40000 ALTER TABLE `mayducdong_qtsx` DISABLE KEYS */;
INSERT INTO `mayducdong_qtsx` VALUES ('1017','6660','6660','6660','0000','10176660666066600000','2019-01-04 11:11:36','2019-01-04 18:11:59','0','8',NULL,'0');
/*!40000 ALTER TABLE `mayducdong_qtsx` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mayducdong_tamdung`
--

DROP TABLE IF EXISTS `mayducdong_tamdung`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mayducdong_tamdung` (
  `ID_lohang` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `date_pause` datetime DEFAULT NULL,
  `date_resume` datetime DEFAULT NULL,
  `date_start` datetime DEFAULT NULL,
  `Trang_thai_start` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Trang_thai_stop` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mayducdong_tamdung`
--

LOCK TABLES `mayducdong_tamdung` WRITE;
/*!40000 ALTER TABLE `mayducdong_tamdung` DISABLE KEYS */;
INSERT INTO `mayducdong_tamdung` VALUES ('10176660666066600000','2019-01-04 11:11:43','2019-01-04 18:11:54','2019-01-04 11:11:36','1','0');
/*!40000 ALTER TABLE `mayducdong_tamdung` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mayducdong_tangca`
--

DROP TABLE IF EXISTS `mayducdong_tangca`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mayducdong_tangca` (
  `ID_lohang` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `date_start_overtime` datetime DEFAULT NULL,
  `date_finish_overtime` datetime DEFAULT NULL,
  `date_start` datetime DEFAULT NULL,
  `Trang_thai_start` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Trang_thai_stop` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mayducdong_tangca`
--

LOCK TABLES `mayducdong_tangca` WRITE;
/*!40000 ALTER TABLE `mayducdong_tangca` DISABLE KEYS */;
INSERT INTO `mayducdong_tangca` VALUES ('10176660666066600000','2019-01-04 18:11:54','2019-01-04 18:11:59','2019-01-04 11:11:36','0','0');
/*!40000 ALTER TABLE `mayducdong_tangca` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-01-04 18:12:23
