-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema documentDB
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `documentDB` ;

-- -----------------------------------------------------
-- Schema documentDB
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `documentDB` DEFAULT CHARACTER SET utf8 ;
USE `documentDB` ;

-- -----------------------------------------------------
-- Table `documentDB`.`DOCUMENT`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `documentDB`.`DOCUMENT` ;

CREATE TABLE IF NOT EXISTS `documentDB`.`DOCUMENT` (
  `ID` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `CODE` INT NOT NULL,
  `TITLE` VARCHAR(45) NULL,
  `CATEGORY_ID` INT NOT NULL,
  `PROCESS` VARCHAR(45) NULL,
  `FILE_TYPE` INT ZEROFILL NOT NULL,
  `FILE_NAME` VARCHAR(45) NOT NULL,
  `FILE_ID` INT NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE INDEX `CODE_UNIQUE` (`CODE` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `documentDB`.`CATEGORY`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `documentDB`.`CATEGORY` ;

CREATE TABLE IF NOT EXISTS `documentDB`.`CATEGORY` (
  `ID` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `NAME` VARCHAR(45) NULL,
  PRIMARY KEY (`ID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `documentDB`.`FILE`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `documentDB`.`FILE` ;

CREATE TABLE IF NOT EXISTS `documentDB`.`FILE` (
  `ID` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `FILE` BLOB NOT NULL,
  PRIMARY KEY (`ID`))
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
