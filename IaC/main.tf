resource "azurerm_resource_group" "resource_group" {
  name     = local.resource_group_name
  location = local.location

}

resource "azurerm_mssql_server" "mssql_server" {
  name                         = local.mssql_server_name
  location                     = azurerm_resource_group.resource_group.location
  resource_group_name          = azurerm_resource_group.resource_group.name
  version                      = "12.0"
  administrator_login          = local.mssql_server_administrator_login
  administrator_login_password = local.mssql_server_administrator_login_password
  minimum_tls_version          = "1.2"

}

resource "azurerm_mssql_database" "mssql_database" {
  name      = local.mssql_database_name
  server_id = azurerm_mssql_server.mssql_server.id
  collation = "SQL_Latin1_General_CP1_CI_AS"

  auto_pause_delay_in_minutes = 60
  max_size_gb                 = 32
  min_capacity                = 0.5
  read_replica_count          = 0
  read_scale                  = false
  sku_name                    = "GP_S_Gen5_1"
  zone_redundant              = false
}

resource "azurerm_app_service_plan" "app_service_plan" {
  name                = local.app_service_plan_name
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  kind                = "Windows"
  reserved            = false

  sku {
    tier = "Standard"
    size = "S1"
  }

}

resource "azurerm_app_service" "app_service_web_crypto" {
  name                = local.app_service_web_crypto_name
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  app_service_plan_id = azurerm_app_service_plan.app_service_plan.id
  identity {
    type = "SystemAssigned"
  }
  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY"        = azurerm_application_insights.application_insights_webapp.instrumentation_key
    "APPLICATIONINSIGHTS_CONNECTION_STRING" = azurerm_application_insights.application_insights_webapp.connection_string
  }
}

resource "azurerm_app_service" "app_service_api_crypto" {

  name                = local.app_service_api_crypto_name
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  app_service_plan_id = azurerm_app_service_plan.app_service_plan.id

  identity {
    type = "SystemAssigned"
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY"        = azurerm_application_insights.application_insights_webapi.instrumentation_key
    "APPLICATIONINSIGHTS_CONNECTION_STRING" = azurerm_application_insights.application_insights_webapi.connection_string
  }
}

resource "azurerm_application_insights" "application_insights_webapp" {
  name                = local.application_insights_webapp_name
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  application_type    = "web"
}

resource "azurerm_application_insights" "application_insights_webapi" {
  name                = local.application_insights_webapi_name
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  application_type    = "web"
}

resource "azurerm_key_vault" "key_vault" {
  name                       = local.key_vault_name
  location                   = azurerm_resource_group.resource_group.location
  resource_group_name        = azurerm_resource_group.resource_group.name
  tenant_id                  = data.azurerm_client_config.current.tenant_id
  sku_name                   = "standard"
  soft_delete_retention_days = 7

}

resource "azurerm_key_vault_access_policy" "key_vault_access_policy_sp" {
  key_vault_id = azurerm_key_vault.key_vault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id

  secret_permissions = [
    "Backup", "Delete", "Get", "List", "Purge", "Recover", "Restore", "Set"
  ]
}

resource "azurerm_key_vault_secret" "key_vault_secret_sqlpassword" {
  name         = "dbadminpassword"
  value        = local.mssql_server_administrator_login_password
  key_vault_id = azurerm_key_vault.key_vault.id

  # prevents race condition when the secret is getting created before the access policy, causing 401
  depends_on = [
    azurerm_key_vault_access_policy.key_vault_access_policy_sp
  ]
}