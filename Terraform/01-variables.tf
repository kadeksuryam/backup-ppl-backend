variable "rg_name_prefix" {
 default = "rg"
 description = "Prefix of resource group name."
}

variable "rg_location" {
  default = "southeastasia"
  description = "Location of the resource group."
}

variable "dns_name_label" {
  default = "cakrawala-id"
  description = "DNS name label."
}