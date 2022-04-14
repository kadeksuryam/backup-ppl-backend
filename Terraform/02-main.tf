resource "random_pet" "rg-name" {
  prefix = var.rg_name_prefix
}

resource "random_id" "randomId" {
  keepers = {
    resource_group = azurerm_resource_group.rg.name
  }

  byte_length = 8
}

resource "azurerm_resource_group" "rg" {
  name = random_pet.rg-name.id
  location = var.rg_location
}

# Storage account for boot diagnostics
resource "azurerm_storage_account" "myStorageAcc" {
  name = "diag${random_id.randomId.hex}"
  location = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  account_tier = "Standard"
  account_replication_type = "LRS"
}

# Create SSH key
resource "tls_private_key" "sshKey" {
  algorithm = "RSA"
  rsa_bits = 4096
}

# VM
resource "azurerm_linux_virtual_machine" "myVM" {
  name = "myVM"
  location = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  network_interface_ids = [azurerm_network_interface.myNIC.id]
  size = "Standard_DS1_v2"

  os_disk {
    name = "myOsDisk"
    caching = "ReadWrite"
    storage_account_type = "Premium_LRS"
  }

  source_image_reference {
    publisher = "Canonical"
    offer = "UbuntuServer"
    sku = "18.04-LTS"
    version = "latest"
  }

  computer_name = "cakrawalaIdVM"
  admin_username = "cakrawalaId"
  disable_password_authentication = true

  user_data = filebase64("Scripts/init-vm.sh")

  admin_ssh_key {
    username = "cakrawalaId"
    public_key = tls_private_key.sshKey.public_key_openssh
  }

  boot_diagnostics {
    storage_account_uri = azurerm_storage_account.myStorageAcc.primary_blob_endpoint
  }
}