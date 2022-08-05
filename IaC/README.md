# CryptoProject

## Table of contents

* [Getting started](#getting-started)
  1. [Prerequisites](#prerequisites)
  2. [Authenticate](#authenticate)
  3. [Create a Service Principal](#create-a-service-principal)
  4. [Create Env Variables](#create-env-variables)
* [Terraform](#terraform)
  1. [Initialize Terraform and perform a Validation](#initialize-terraform-and-perform-a-validation)
  2. [Overview of your Infrastructure](#overview-of-your-infrastructure)
  3. [Apply to Create Infrastructure](#apply-to-create-infrastructure)
  4. [Verify your created Infrastructure on the Azure Portal](#verify-your-created-infrastructure-on-the-azure-portal)
  5. [Destroy your resources when you no longer need them](#destroy-your-resources-when-you-no-longer-need-them)
* [Contributors](#contributors)

## Getting started

### Prerequisites

- [Azure Account](https://azure.microsoft.com/en-us/free/)
- [Terraform](https://www.terraform.io/downloads)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

You can click on the links to create or install them according to your specific OS. 

### Authenticate

Login with Azure CLI.

```
az login
az account set --subscription "<SUBSCRIPTION_ID>"
```

### Create a Service Principal

Open PowerShell and run the following cmd, do not forget to replace <SUBSCRIPTION_ID>

```
az ad sp create-for-rbac --role="Contributor" --scopes="/subscriptions/<SUBSCRIPTION_ID>" --name="crypto-sp"
```

Do NOT clean your terminal or save those values in a Notepad, as you will be using them in a moment.
Or you can add > azuresp.json add the end of the cmd to save them in a json file that is ignored by the [.gitignore](../.gitignore).
Later you can automate the tasks to take your credentials from there.

### Create Env Variables

The following cmds use PowerShell env variables, if you are using Linux or macOS just replace them accordingly.

In PowerShell, create environmental variables to store the sensitive values you got in the previous step and the values that will be passed as Terraform arguments. 

```
$Env:ARM_CLIENT_ID = "<APPID_VALUE>"
$Env:ARM_CLIENT_SECRET = "<PASSWORD_VALUE>"
$Env:ARM_SUBSCRIPTION_ID = "<SUBSCRIPTION_ID>"
$Env:ARM_TENANT_ID = "<TENANT_VALUE>"
$Env:MSSQL_SERVER_ADMIN = "<YOUR_DB_ADMIN>"
$Env:MSSQL_SERVER_ADMIN_PASSWORD = "<YOUR_DB_PASSWORD>
$Env:AZ_REGION = "<AZURE_REGION>"
$Env:RG_PREFIX = "<RESOURCE_PREFIX>"
```

For example, I would like to add *crypto* as my resource prefix and I used *germanywestcentral* azure region. So I set

```
$Env:RG_PREFIX = "crypto"
$Env:AZ_REGION = "germanywestcentral"
```

## Terraform

### Initialize Terraform and perform a Validation

```
terraform init
```

```
terraform fmt
```

```
terraform validate
```

### Overview of your Infrastructure

```
terraform plan -var="location=$Env:AZ_REGION" -var="resources_prefix=$Env:RG_PREFIX" -var="mssql_server_administrator_login=$Env:MSSQL_SERVER_ADMIN" -var="mssql_server_administrator_login_password=$Env:MSSQL_SERVER_ADMIN_PASSWORD"
```

### Apply to Create Infrastructure

```
terraform apply -var="location=$Env:AZ_REGION" -var="resources_prefix=$Env:RG_PREFIX" -var="mssql_server_administrator_login=$Env:MSSQL_SERVER_ADMIN" -var="mssql_server_administrator_login_password=$Env:MSSQL_SERVER_ADMIN_PASSWORD"
```

Review the resources that will be created and then type *yes*.

![](../assets/img/iac/terraform%20outputs.PNG)

One feature added to the [outputs.tf](outputs.tf) file is that some values are set as sensitive. Such as the MS SQL Server password.

### Verify your created Infrastructure on the Azure Portal

![](../assets/img/iac/azure%20portal%20prod%20rg.PNG)

As it can be seen, the resources were created using the environmental variables passed as Terraform arguments. For example, the resources are all created in Germany West Central and they use the *crypto* prefix in their names. This prefix is added in the [locals.tf](locals.tf) file in addition to the suffix, which could be taken from arguments if needed.

### Destroy your resources when you no longer need them

```
terraform destroy -var="location=$Env:AZ_REGION" -var="resources_prefix=$Env:RG_PREFIX" -var="mssql_server_administrator_login=$Env:MSSQL_SERVER_ADMIN" -var="mssql_server_administrator_login_password=$Env:MSSQL_SERVER_ADMIN_PASSWORD"
```

Type *yes*

## Contributors

* Oscar Santos
