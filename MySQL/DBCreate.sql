-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`CATEGORY`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`CATEGORY` ;

CREATE TABLE IF NOT EXISTS `mydb`.`CATEGORY` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `NAME` VARCHAR(45) NULL,
  PRIMARY KEY (`ID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`DOCUMENT`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`DOCUMENT` ;

CREATE TABLE IF NOT EXISTS `mydb`.`DOCUMENT` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `CODE` INT NULL,
  `title` VARCHAR(45) NULL,
  `PROCESS` VARCHAR(45) NULL,
  `FILE` VARCHAR(45) NULL,
  `CATEGORY_ID` INT NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE INDEX `codigo_UNIQUE` (`CODE` ASC) VISIBLE,
  UNIQUE INDEX `ID_UNIQUE` (`ID` ASC) VISIBLE,
  INDEX `fk_DOCUMENT_CATEGORY_idx` (`CATEGORY_ID` ASC) VISIBLE,
  CONSTRAINT `fk_DOCUMENT_CATEGORY`
    FOREIGN KEY (`CATEGORY_ID`)
    REFERENCES `mydb`.`CATEGORY` (`ID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
