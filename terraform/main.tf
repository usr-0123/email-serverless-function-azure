provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "email_rg" {
  name     = "email-function-rg"
  location = "westeurope"
}

resource "azurerm_storage_account" "email_storage" {
  name                     = "emailfunctionstorage123"
  resource_group_name      = azurerm_resource_group.email_rg.name
  location                 = azurerm_resource_group.email_rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_service_plan" "function_plan" {
  name                = "email-function-plan"
  location            = azurerm_resource_group.email_rg.location
  resource_group_name = azurerm_resource_group.email_rg.name
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "email_function" {
  name                = "booking-email-function"
  location            = azurerm_resource_group.email_rg.location
  resource_group_name = azurerm_resource_group.email_rg.name

  service_plan_id = azurerm_service_plan.function_plan.id

  storage_account_name       = azurerm_storage_account.email_storage.name
  storage_account_access_key = azurerm_storage_account.email_storage.primary_access_key

  site_config {
    application_stack {
      dotnet_version = "8.0"
    }
  }

  app_settings = {
    "FUNCTIONS_WORKER_RUNTIME" = "dotnet"
    "SENDGRID_API_KEY"         = "CHANGE_ME"
  }
}