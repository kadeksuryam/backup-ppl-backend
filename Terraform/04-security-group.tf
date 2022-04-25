# Security Group
resource "azurerm_network_security_group" "mySG" {
  name = "mySG"
  location = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
}

# SSH Security Rule 
resource "azurerm_network_security_rule" "sshSecurityRule" {
  name = "SSH"
  priority = 1001
  direction = "Inbound"
  access = "Allow"
  protocol = "Tcp"
  source_port_range = "*"
  destination_port_range = "22"
  source_address_prefix = "*"
  destination_address_prefix = "*"
  resource_group_name = azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.mySG.name
}

# HTTP Security Rule
resource "azurerm_network_security_rule" "httpSecurityRule" {
  name = "HTTP"
  priority = 400
  direction = "Inbound"
  access = "Allow"
  protocol = "Tcp"
  source_port_range = "*"
  destination_port_range = "80"
  source_address_prefix = "*"
  destination_address_prefix = "*"
  resource_group_name = azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.mySG.name
}

# HTTPS Security Rule
resource "azurerm_network_security_rule" "httpsSecurityRule" {
  name = "HTTPS"
  priority = 500
  direction = "Inbound"
  access = "Allow"
  protocol = "Tcp"
  source_port_range = "*"
  destination_port_range = "443"
  source_address_prefix = "*"
  destination_address_prefix = "*"
  resource_group_name = azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.mySG.name
}

# DB Port Security Rule
resource "azurerm_network_security_rule" "dbPortSecurityRule" {
  name = "DB PORT"
  priority = 600
  direction = "Inbound"
  access = "Allow"
  protocol = "Tcp"
  source_port_range = "*"
  destination_port_range = "5432"
  source_address_prefix = "*"
  destination_address_prefix = "*"
  resource_group_name = azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.mySG.name
}

# Connect SG to NIC
resource "azurerm_network_interface_security_group_association" "myNICtoSGCon" {
  network_interface_id = azurerm_network_interface.myNIC.id
  network_security_group_id = azurerm_network_security_group.mySG.id
}