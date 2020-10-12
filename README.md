<p align="center">
<img
    src="http://blockapi.quraswallet.org/quras/img/logo1.png"
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

### Build Environment

Install .NET Framework 4.7.1 or higher versions. You can download it from https://dotnet.microsoft.com/download/dotnet-framework/net471.

### Installation

Install the package using:

```js
npm install
```

### run

```js
node index.js --run
```

### Docs

We use Docusaurus for our docs website. The docs are stores in `./docs` while the main website and its configuration is in `./website`.

```bash
cd website
npm install
npm run start
```

## Configuration

### Database Configuration

You can configure database connect information in 'config.json'

```
"development" : {
    "database": {
      "host": "localhost",
      "user": "root",
      "password": "",
      "database": "quras_db"
    },
    ... ...
},
"production" : {
    "database": {
      "host": "localhost",
      "user": "root",
      "password": "",
      "database": "quras_db"
    },
    ... ...
}
```

### Node Configuration

You can configure node url in 'config.json'.

```
"development" : {
    ... ...
    "node_url": "http://localhost:8989",
    ... ...
},
"production" : {
    ... ...
    "node_url": "http://localhost:8989",
    ... ...
}
``` 

## Runtime Environment
|OS Type|Version|
|---|---|
|Windows|any|