output "resource_group_id" {
  value       = azurerm_resource_group.resource_group.id
  description = "The ID of the Resource Group."
}

output "mssql_server_fqdn" {
  value       = azurerm_mssql_server.mssql_server.fully_qualified_domain_name
  description = "The fully qualified domain name of the Azure SQL Server (e.g. myServerName.database.windows.net)"
  sensitive   = true
}

output "mssql_database_name" {
  value       = azurerm_mssql_database.mssql_database.name
  description = "The Name of the MS SQL Database."
}

output "app_service_plan_id" {
  value       = azurerm_app_service_plan.app_service_plan.id
  description = "The ID of the Resource Group."
}

output "app_service_web_id" {
  value = azurerm_app_service.app_service_web_crypto.id
}

output "app_service_api_id" {
  value = azurerm_app_service.app_service_api_crypto.id
}

output "application_insights_web_app_id" {
  value       = azurerm_application_insights.application_insights_webapp.app_id
  description = "The App ID associated with this Application Insights component."
}

output "application_insights_api_app_id" {
  value       = azurerm_application_insights.application_insights_webapi.app_id
  description = "The App ID associated with this Application Insights component."
}

output "key_vault_vault_uri" {
  value     = azurerm_key_vault.key_vault.vault_uri
  sensitive = true
}

output "key_vault_access_policy_kvid" {
  value       = azurerm_key_vault_access_policy.key_vault_access_policy_sp.id
  description = "The ID of the Key Vault"
}

output "key_vault_secret_id" {
  value       = azurerm_key_vault_secret.key_vault_secret_sqlpassword.id
  description = "The Key Vault Secret ID."
}

output "mssql_server_admin_password" {
  value     = azurerm_mssql_server.mssql_server.administrator_login_password
  sensitive = true
}