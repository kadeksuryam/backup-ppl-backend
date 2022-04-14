
# Virtual Network
resource "azurerm_virtual_network" "myVnet" {
  name = "myVnet"
  address_space = ["10.0.0.0/16"]
  location = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
}

# Subnet
resource "azurerm_subnet" "mySubnet" {
  name = "mySubnet"
  resource_group_name = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.myVnet.name
  address_prefixes = ["10.0.1.0/24"]
}

# Public IPs
resource "azurerm_public_ip" "myPublicIP" {
  name = "myPublicIP"
  location = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  allocation_method = "Dynamic"
  domain_name_label = var.dns_name_label
}

# Network Interface
resource "azurerm_network_interface" "myNIC" {
  name = "myNIC"
  location = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  ip_configuration {
    name = "myNicConfig"
    subnet_id = azurerm_subnet.mySubnet.id
    private_ip_address_allocation = "Dynamic"
    public_ip_address_id = azurerm_public_ip.myPublicIP.id
  }
}
