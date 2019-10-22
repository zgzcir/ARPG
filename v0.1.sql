/*
 Navicat Premium Data Transfer

 Source Server         : mysql8.0
 Source Server Type    : MySQL
 Source Server Version : 80016
 Source Host           : localhost:3306
 Source Schema         : arpg

 Target Server Type    : MySQL
 Target Server Version : 80016
 File Encoding         : 65001

 Date: 22/10/2019 20:22:18
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for account
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `acct` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `pass` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `hp` int(20) NOT NULL,
  `coin` int(11) NOT NULL,
  `power` int(11) NOT NULL,
  `crystal` int(11) NOT NULL,
  `diamond` int(11) NOT NULL,
  `level` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `pd` int(11) NOT NULL COMMENT '物理防御',
  `sd` int(11) NOT NULL COMMENT '法术防御',
  `pa` int(11) NOT NULL COMMENT '物理攻击',
  `sa` int(11) NOT NULL COMMENT '法术攻击',
  `dodge` int(11) NOT NULL,
  `pierce` int(11) NOT NULL COMMENT '破甲',
  `critical` int(11) NOT NULL DEFAULT 11 COMMENT '暴击率',
  `guideid` int(11) NOT NULL,
  `strenarr` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `time` bigint(11) NOT NULL,
  `taskrewardarr` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `mission` int(11) NOT NULL,
  `kanban` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 135 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
