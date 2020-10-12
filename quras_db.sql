-- phpMyAdmin SQL Dump
-- version 4.9.0.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sep 23, 2020 at 08:32 AM
-- Server version: 10.3.15-MariaDB
-- PHP Version: 7.3.6

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `quras_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `anonymous_contract_transaction`
--

CREATE TABLE `anonymous_contract_transaction` (
  `txid` varchar(255) NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `approver_list`
--

CREATE TABLE `approver_list` (
  `scripthash` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `publickey` varchar(265) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `status` int(5) NOT NULL DEFAULT 1 COMMENT '0: pending, 1: registered, 2: unregistered'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `approve_download_transaction`
--

CREATE TABLE `approve_download_transaction` (
  `txid` varchar(255) NOT NULL,
  `approve_address` varchar(255) NOT NULL,
  `download_address` varchar(255) NOT NULL,
  `dtx_hash` varchar(255) NOT NULL,
  `approve_state` tinyint(1) NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `blocks`
--

CREATE TABLE `blocks` (
  `hash` varchar(256) CHARACTER SET utf8 NOT NULL,
  `size` int(11) NOT NULL,
  `version` int(11) NOT NULL,
  `prev_block_hash` varchar(256) CHARACTER SET utf8 NOT NULL,
  `merkle_root` varchar(256) CHARACTER SET utf8 NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `nonce` int(11) NOT NULL,
  `current_consensus` varchar(256) CHARACTER SET utf8 NOT NULL,
  `next_consensus` varchar(256) CHARACTER SET utf8 NOT NULL,
  `script` varchar(512) CHARACTER SET utf8 NOT NULL,
  `tx_count` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `claim_transaction`
--

CREATE TABLE `claim_transaction` (
  `txid` varchar(255) NOT NULL,
  `claims` text NOT NULL,
  `address` text NOT NULL,
  `amount` double NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `contract_transaction`
--

CREATE TABLE `contract_transaction` (
  `txid` varchar(255) NOT NULL,
  `_from` text NOT NULL,
  `_to` text NOT NULL,
  `asset` text NOT NULL,
  `name` varchar(256) NOT NULL,
  `amount` double NOT NULL,
  `fee` double NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `download_request_transaction`
--

CREATE TABLE `download_request_transaction` (
  `txid` varchar(255) NOT NULL,
  `file_name` text NOT NULL,
  `file_description` text NOT NULL,
  `file_url` longtext NOT NULL,
  `pay_amount` double NOT NULL,
  `upload_address` varchar(255) NOT NULL,
  `download_address` varchar(255) NOT NULL,
  `file_hash` varchar(255) NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `enrollment_transaction`
--

CREATE TABLE `enrollment_transaction` (
  `txid` varchar(255) NOT NULL,
  `pubkey` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `holders`
--

CREATE TABLE `holders` (
  `id` int(11) NOT NULL,
  `address` varchar(256) NOT NULL,
  `name` varchar(256) NOT NULL,
  `asset` varchar(256) NOT NULL,
  `balance` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `invocation_transaction`
--

CREATE TABLE `invocation_transaction` (
  `txid` varchar(255) NOT NULL,
  `address` text NOT NULL,
  `script` text NOT NULL,
  `gas` text NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `issue_transaction`
--

CREATE TABLE `issue_transaction` (
  `txid` varchar(255) NOT NULL,
  `address` text DEFAULT NULL,
  `asset` text NOT NULL,
  `name` varchar(256) NOT NULL,
  `_to` text NOT NULL,
  `amount` double NOT NULL,
  `fee` double DEFAULT 0,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `miner_transaction`
--

CREATE TABLE `miner_transaction` (
  `txid` varchar(255) NOT NULL,
  `address` text DEFAULT NULL,
  `_to` text DEFAULT NULL,
  `amount` double DEFAULT NULL,
  `nonce` int(20) NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `nodes`
--

CREATE TABLE `nodes` (
  `id` int(11) NOT NULL,
  `url` text NOT NULL,
  `height` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `pay_file_transaction`
--

CREATE TABLE `pay_file_transaction` (
  `txid` varchar(255) NOT NULL,
  `download_address` text NOT NULL,
  `upload_address` text NOT NULL,
  `asset` text NOT NULL,
  `name` varchar(256) NOT NULL,
  `amount` double NOT NULL,
  `fee` double NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(255) NOT NULL,
  `dtx_hash` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `pay_upload_transaction`
--

CREATE TABLE `pay_upload_transaction` (
  `txid` varchar(255) NOT NULL,
  `utxid` varchar(255) NOT NULL,
  `upload_size` double NOT NULL,
  `from_address` varchar(255) NOT NULL,
  `to_address` varchar(255) NOT NULL,
  `asset` varchar(255) NOT NULL,
  `name` varchar(32) NOT NULL,
  `amount` double NOT NULL,
  `fee` double NOT NULL,
  `time` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `block_number` bigint(20) NOT NULL,
  `hash` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `publish_transaction`
--

CREATE TABLE `publish_transaction` (
  `txid` varchar(255) NOT NULL,
  `contract_code_hash` text NOT NULL,
  `contract_code_script` text NOT NULL,
  `contract_code_parameters` text NOT NULL,
  `contract_code_returntype` text NOT NULL,
  `contract_needstorage` text NOT NULL,
  `contract_name` text NOT NULL,
  `contract_version` text NOT NULL,
  `contract_author` text NOT NULL,
  `contract_email` text NOT NULL,
  `contract_description` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `register_multisign_transaction`
--

CREATE TABLE `register_multisign_transaction` (
  `txid` varchar(255) NOT NULL,
  `scripthash` text NOT NULL,
  `address` text NOT NULL,
  `pubkeys` text NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `register_transaction`
--

CREATE TABLE `register_transaction` (
  `txid` varchar(255) NOT NULL,
  `type` text NOT NULL,
  `name` text NOT NULL,
  `amount` text NOT NULL,
  `_precision` text NOT NULL,
  `owner` text NOT NULL,
  `admin` text NOT NULL,
  `issure` text NOT NULL,
  `tfee_min` double DEFAULT NULL,
  `tfee_max` double DEFAULT NULL,
  `afee` double DEFAULT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `ring_confident_transaction`
--

CREATE TABLE `ring_confident_transaction` (
  `txid` varchar(255) NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `smartcontract_member_list`
--

CREATE TABLE `smartcontract_member_list` (
  `scripthash` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `publickey` varchar(265) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `status` int(11) NOT NULL DEFAULT 1 COMMENT '0: pending, 1: registered, 2: unregistered'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `state_transaction`
--

CREATE TABLE `state_transaction` (
  `txid` varchar(255) NOT NULL,
  `from_address` varchar(255) NOT NULL,
  `to_address` varchar(255) NOT NULL,
  `upload_hash` varchar(260) NOT NULL,
  `state_script` text NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `status`
--

CREATE TABLE `status` (
  `id` int(11) NOT NULL,
  `current_block_number` int(11) NOT NULL,
  `last_block_time` int(11) NOT NULL,
  `block_version` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `storage_wallets`
--

CREATE TABLE `storage_wallets` (
  `id` int(11) NOT NULL,
  `txid` varchar(255) NOT NULL,
  `address` varchar(255) NOT NULL,
  `storage_size` double NOT NULL,
  `current_size` double NOT NULL DEFAULT 0,
  `gurantee_amount_per_gb` double NOT NULL,
  `pay_amount_per_gb` double NOT NULL,
  `end_time` int(11) DEFAULT 0,
  `rate` double NOT NULL DEFAULT 5
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `transactions`
--

CREATE TABLE `transactions` (
  `txid` varchar(255) CHARACTER SET utf8 NOT NULL,
  `size` int(11) NOT NULL,
  `tx_type` varchar(255) CHARACTER SET utf8 NOT NULL,
  `version` int(11) NOT NULL,
  `attribute` text CHARACTER SET utf8 NOT NULL,
  `vin` text CHARACTER SET utf8 NOT NULL,
  `vout` text CHARACTER SET utf8 NOT NULL,
  `sys_fee` double NOT NULL,
  `net_fee` double NOT NULL,
  `scripts` text CHARACTER SET utf8 NOT NULL,
  `nonce` int(11) NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `tx_history`
--

CREATE TABLE `tx_history` (
  `txid` text NOT NULL,
  `time` text NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(256) NOT NULL,
  `tx_type` text NOT NULL,
  `claim_transaction_address` text NOT NULL,
  `claim_transaction_amount` double NOT NULL,
  `contract_transaction_from` text NOT NULL,
  `contract_transaction_to` text NOT NULL,
  `contract_transaction_asset` text NOT NULL,
  `contract_transaction_amount` double NOT NULL,
  `contract_transaction_fee` double NOT NULL,
  `invocation_transaction_address` text NOT NULL,
  `gas` double NOT NULL,
  `issue_transaction_address` text NOT NULL,
  `issue_transaction_to` text NOT NULL,
  `issue_transaction_asset` text NOT NULL,
  `issue_transaction_amount` double NOT NULL,
  `issue_transaction_fee` double NOT NULL,
  `miner_transaction_address` text NOT NULL,
  `miner_transaction_to` text NOT NULL,
  `miner_transaction_amount` double NOT NULL,
  `name` varchar(256) NOT NULL DEFAULT 'XQG',
  `uploadrequest_transaction_file_name` text NOT NULL,
  `uploadrequest_transaction_file_description` text NOT NULL,
  `uploadrequest_transaction_file_url` longtext NOT NULL,
  `uploadrequest_transaction_pay_amount` double NOT NULL DEFAULT 0,
  `uploadrequest_transaction_upload_address` varchar(255) NOT NULL,
  `uploadrequest_transaction_verifiers` text NOT NULL,
  `uploadrequest_transaction_duration_days` int(11) NOT NULL,
  `downloadrequest_transaction_file_name` text NOT NULL,
  `downloadrequest_transaction_file_description` text NOT NULL,
  `downloadrequest_transaction_file_url` text NOT NULL,
  `downloadrequest_transaction_pay_amount` double NOT NULL,
  `downloadrequest_transaction_upload_address` varchar(255) NOT NULL,
  `downloadrequest_transaction_download_address` varchar(255) NOT NULL,
  `downloadrequest_transaction_tx_hash` varchar(256) NOT NULL,
  `approvedownload_transaction_approve_address` varchar(255) NOT NULL,
  `approvedownload_transaction_download_address` varchar(255) NOT NULL,
  `approvedownload_transaction_tx_hash` varchar(256) NOT NULL,
  `approvedownload_transaction_approve_state` tinyint(1) NOT NULL,
  `payfile_transaction_download_address` varchar(255) NOT NULL,
  `payfile_transaction_upload_address` varchar(255) NOT NULL,
  `payfile_transaction_name` varchar(11) NOT NULL,
  `payfile_transaction_pay_amount` double NOT NULL,
  `payfile_transaction_fee` double NOT NULL DEFAULT 0,
  `payfile_transaction_asset` text NOT NULL,
  `payfile_transaction_tx_hash` varchar(256) NOT NULL,
  `storage_wallet_address` varchar(255) NOT NULL,
  `storage_wallet_size` double NOT NULL,
  `storage_wallet_gurantee_amount_per_gb` double NOT NULL,
  `storage_wallet_pay_amount_per_gb` double NOT NULL,
  `storage_wallet_end_tim` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `payupload_transaction_utxid` varchar(255) NOT NULL,
  `payupload_transaction_size` double NOT NULL,
  `payupload_transaction_from` varchar(255) NOT NULL,
  `payupload_transaction_to` varchar(255) NOT NULL,
  `payupload_transaction_asset` varchar(255) NOT NULL,
  `payupload_transaction_name` varchar(32) NOT NULL,
  `payupload_transaction_amount` double NOT NULL,
  `payupload_transaction_fee` double NOT NULL,
  `state_from_address` varchar(255) NOT NULL,
  `state_to_address` varchar(255) NOT NULL,
  `state_upload_hash` varchar(260) NOT NULL,
  `state_state_script` text NOT NULL,
  `multisign_scripthash` text NOT NULL,
  `multisign_address` text NOT NULL,
  `multisign_pubkeys` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `upload_request_transaction`
--

CREATE TABLE `upload_request_transaction` (
  `txid` varchar(255) NOT NULL,
  `file_name` text NOT NULL,
  `file_description` text NOT NULL,
  `file_url` longtext NOT NULL,
  `pay_amount` double NOT NULL,
  `upload_address` varchar(256) NOT NULL,
  `file_verifiers` text NOT NULL,
  `duration_days` int(11) NOT NULL,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` int(11) NOT NULL,
  `status` int(5) NOT NULL DEFAULT 0 COMMENT '0: Available, 1: Canceled'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `utxos`
--

CREATE TABLE `utxos` (
  `txid` varchar(255) NOT NULL,
  `tx_out_index` int(11) NOT NULL,
  `asset` varchar(255) NOT NULL,
  `name` varchar(256) NOT NULL,
  `value` double NOT NULL DEFAULT 0,
  `address` varchar(255) NOT NULL,
  `status` varchar(255) NOT NULL,
  `claimed` double NOT NULL DEFAULT 0,
  `time` int(11) NOT NULL,
  `block_number` int(11) NOT NULL,
  `hash` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `anonymous_contract_transaction`
--
ALTER TABLE `anonymous_contract_transaction`
  ADD PRIMARY KEY (`txid`);

--
-- Indexes for table `approver_list`
--
ALTER TABLE `approver_list`
  ADD PRIMARY KEY (`scripthash`);

--
-- Indexes for table `blocks`
--
ALTER TABLE `blocks`
  ADD PRIMARY KEY (`block_number`);

--
-- Indexes for table `claim_transaction`
--
ALTER TABLE `claim_transaction`
  ADD PRIMARY KEY (`txid`);

--
-- Indexes for table `enrollment_transaction`
--
ALTER TABLE `enrollment_transaction`
  ADD PRIMARY KEY (`txid`);

--
-- Indexes for table `holders`
--
ALTER TABLE `holders`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `invocation_transaction`
--
ALTER TABLE `invocation_transaction`
  ADD PRIMARY KEY (`txid`);

--
-- Indexes for table `nodes`
--
ALTER TABLE `nodes`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `publish_transaction`
--
ALTER TABLE `publish_transaction`
  ADD PRIMARY KEY (`txid`);

--
-- Indexes for table `register_transaction`
--
ALTER TABLE `register_transaction`
  ADD PRIMARY KEY (`txid`);

--
-- Indexes for table `ring_confident_transaction`
--
ALTER TABLE `ring_confident_transaction`
  ADD PRIMARY KEY (`txid`);

--
-- Indexes for table `smartcontract_member_list`
--
ALTER TABLE `smartcontract_member_list`
  ADD PRIMARY KEY (`scripthash`);

--
-- Indexes for table `status`
--
ALTER TABLE `status`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `storage_wallets`
--
ALTER TABLE `storage_wallets`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `transactions`
--
ALTER TABLE `transactions`
  ADD PRIMARY KEY (`txid`);

--
-- Indexes for table `utxos`
--
ALTER TABLE `utxos`
  ADD PRIMARY KEY (`txid`,`tx_out_index`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `holders`
--
ALTER TABLE `holders`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `nodes`
--
ALTER TABLE `nodes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `storage_wallets`
--
ALTER TABLE `storage_wallets`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
