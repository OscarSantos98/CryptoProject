variable "location" {
  description = ""
  type        = string
}

variable "resources_prefix" {
  description = ""
  type        = string
  default     = null
}

variable "mssql_server_administrator_login" {
  description = ""
  type        = string
  default     = null
}

variable "mssql_server_administrator_login_password" {
  description = ""
  type        = string
  default     = null
  sensitive   = true
}