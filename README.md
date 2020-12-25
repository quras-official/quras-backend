<p align="center">
<img
    src="http://blockapi.quras.io/quras/img/logo1.png"
    width="200px">
</p>

## Table of Contents
1. [Overview](#overview)
2. [Getting started](#getting-started)
3. [Configuration](#configuration)
4. [Runtime Environment](#runtime-environment)

## Overview

This the Quras back-end service for operating the db of the Quras blockchain.
This service instruct the db for supplying the apis.
Before starting this service, you have to configure the MySQL DB server.

## Getting started

### Runtime Environment

Install .NET Framework 4.7.1 or higher versions. (Do not require if you are using Windows 10+.)
You can download it from https://dotnet.microsoft.com/download/dotnet-framework/net471.

## Configuration

### Database Configuration

You can configure database connect information in 'config.json'

```
{
	... ...
	"DbHost": "127.0.0.1",
	"DbPort": "3306",
	"DbUser": "root",
	"DbPassword": "",
	"DbName" :  "quras_db"
}
```

### Node Configuration

You can configure node url in 'config.json'.

```
{
    "UriPrefix": "https://rpc.quras.io:10030",
    ... ...
}
``` 

## Runtime Environment
|OS Type|Version|
|---|---|
|Windows|any|