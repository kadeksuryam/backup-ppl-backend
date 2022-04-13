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


# Connect SG to NIC
resource "azurerm_network_interface_security_group_association" "myNICtoSGCon" {
  network_interface_id = azurerm_network_interface.myNIC.id
  network_security_group_id = azurerm_network_security_group.mySG.id
}