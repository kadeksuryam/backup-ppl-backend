output "resource_group_name" {
  value = azurerm_resource_group.rg.name
}

output "public_ip_address" {
  value = azurerm_public_ip.myPublicIP.ip_address
}

output "tls_private_key" {
  value     = tls_private_key.sshKey.private_key_pem
  sensitive = true
}

output "dns_name_label_name" {
  value = var.dns_name_label
}