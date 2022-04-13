#!/bin/bash
RESOURCE_GROUP_NAME=tfstatecakrawalaid
STORAGE_ACCOUNT_NAME=tfstatecakrawalaid$RANDOM
CONTAINER_NAME=tfstatecakrawalaid
KEY_NAME=terraform.tfstate

# Create resource group
az group create --name $RESOURCE_GROUP_NAME --location southeastasia

# Create storage account
az storage account create --resource-group $RESOURCE_GROUP_NAME --name $STORAGE_ACCOUNT_NAME --sku Standard_LRS --encryption-services blob

# Create blob container
az storage container create --name $CONTAINER_NAME --account-name $STORAGE_ACCOUNT_NAME

# Output the config
mydir="${0%/*}"

cat > $mydir/../Config/terraform-backend.conf <<EOL
resource_group_name = "${RESOURCE_GROUP_NAME}"
storage_account_name = "${STORAGE_ACCOUNT_NAME}"
container_name = "${CONTAINER_NAME}"
key = "${KEY_NAME}"
EOL

   