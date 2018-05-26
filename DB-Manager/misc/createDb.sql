CREATE DATABASE IF NOT EXISTS `game_vision`;



CREATE TABLE IF NOT EXISTS `divisions` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) ,
  `country` varchar(50) ,
  `sign` varchar(50) NOT NULL DEFAULT '0',
  UNIQUE KEY `sign` (`sign`),
  KEY `id` (`id`)
)



CREATE TABLE IF NOT EXISTS `games` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `HomeTeam` varchar(50) NOT NULL DEFAULT '0',
  `AwayTeam` varchar(50) NOT NULL DEFAULT '0',
  `FTHG` int(11) NOT NULL DEFAULT '0',
  `FTAG` int(11) NOT NULL DEFAULT '0',
  `FTR` varchar(50) NOT NULL DEFAULT '0',
  `HTHG` int(11) NOT NULL DEFAULT '0',
  `HTAG` int(11) NOT NULL DEFAULT '0',
  `HTR` varchar(50) NOT NULL DEFAULT '0',
  `HS` int(11) NOT NULL DEFAULT '0',
  `AS` int(11) NOT NULL DEFAULT '0',
  `HST` int(11) NOT NULL DEFAULT '0',
  `AST` int(11) NOT NULL DEFAULT '0',
  `HF` int(11) NOT NULL DEFAULT '0',
  `AF` int(11) NOT NULL DEFAULT '0',
  `HC` int(11) NOT NULL DEFAULT '0',
  `AC` int(11) NOT NULL DEFAULT '0',
  `HY` int(11) NOT NULL DEFAULT '0',
  `AY` int(11) NOT NULL DEFAULT '0',
  `HR` int(11) NOT NULL DEFAULT '0',
  `AR` int(11) NOT NULL DEFAULT '0',
  `DIV` varchar(50) NOT NULL,
  `DATE` date NOT NULL,
   UNIQUE KEY `id` (`id`),
   UNIQUE KEY `DATE` (`DATE`,`HomeTeam`,`AwayTeam`)
)


CREATE TABLE IF NOT EXISTS `teams` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL DEFAULT '0',
  `divisionSign` varchar(50) DEFAULT NULL,
  `bigclub` tinyint(4) DEFAULT NULL,
  UNIQUE KEY `name` (`name`),
  KEY `id` (`id`)
)
