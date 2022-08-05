locals {
  resources_prefix    = var.resources_prefix
  location            = var.location
  resource_group_name = "${local.resources_prefix}Project-PROD"

  key_vault_name                            = "${local.resources_prefix}project-kv-prod"
  application_insights_webapp_name          = "${local.resources_prefix}webapp-prod"
  application_insights_webapi_name          = "${local.resources_prefix}webapi-prod"
  mssql_server_name                         = "${local.resources_prefix}sqlserver-prod"
  mssql_server_administrator_login          = var.mssql_server_administrator_login
  mssql_server_administrator_login_password = var.mssql_server_administrator_login_password
  mssql_database_name                       = "cryptodb_prod"
  app_service_plan_name                     = "${local.resources_prefix}webapp-asp-prod"
  app_service_web_crypto_name               = "${local.resources_prefix}webapp-prod"
  app_service_api_crypto_name               = "${local.resources_prefix}webapi-prod"
}