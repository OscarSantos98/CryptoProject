# CryptoProject

## Create a service principal

Open Powershell and run the following cmd, don't forget to replace <SUBSCRIPTION_ID>

```
az ad sp create-for-rbac --role="Contributor" --scopes="/subscriptions/<SUBSCRIPTION_ID>"
```

## Authentication and Env variables

Login with Azure CLI

```
az login
az account set --subscription "<SUBSCRIPTION_ID>"
```

Create environmental variables to store sensitive values.

```
$Env:ARM_CLIENT_ID = "<APPID_VALUE>"
$Env:ARM_CLIENT_SECRET = "<PASSWORD_VALUE>"
$Env:ARM_SUBSCRIPTION_ID = "<SUBSCRIPTION_ID>"
$Env:ARM_TENANT_ID = "<TENANT_VALUE>"
$Env:MSSQL_SERVER_ADMIN_PASSWORD = "<YOUR_DB_PASSWORD>
$Env:AZ_REGION = "<AZURE_REGION>"
```

For example, I used germanywestcentral region.

## Initialize Terraform and perform a validation

```
terraform init
```

```
terraform fmt
```

```
terraform validate
```

## Overview of your infrastructure

```
terraform plan -var="location=$Env:AZ_REGION" -var="resources_prefix=crypto" -var="mssql_server_administrator_login=dbadmin" -var="mssql_server_administrator_login_password=$Env:MSSQL_SERVER_ADMIN_PASSWORD"
```

## Apply to create infrastructure

```
terraform apply -var="location=$Env:AZ_REGION" -var="resources_prefix=crypto" -var="mssql_server_administrator_login=dbadmin" -var="mssql_server_administrator_login_password=$Env:MSSQL_SERVER_ADMIN_PASSWORD"
```

## Destroy your resources when you no longer need them

```
terraform destroy -var="location=$Env:AZ_REGION" -var="resources_prefix=crypto" -var="mssql_server_administrator_login=dbadmin" -var="mssql_server_administrator_login_password=$Env:MSSQL_SERVER_ADMIN_PASSWORD"
```