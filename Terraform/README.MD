## Terraform Script for Provisioning Azure VM + Docker Installed

## Requirements
- Terraform 0.13.7
- Azure CLI & Already Logged In

## How to Provision the VM
- Run `./Scripts/init-storage-account-tfstate.sh`, if error makes sure this script is executable, you can change it by `chmod +x ./Scripts/init-storage-account-tfstate.sh`
- Run `terraform init -backend-config=./Config/terraform-backend.conf`
- Run `terraform apply`

## How to Decommence the VM & Its dependencies
- Run `terraform destroy`

## How to Connect the VM
- From Portal Azure, find the IP address of the VM
- Generate SSH key by `terraform output tls_private_key > ~/.ssh/azurevmkey`
- Change key permission to Read Only by `chmod 400 ~/.ssh/azurevmkey`
- Run `ssh -i ~/.ssh/azurevmkey vmName@vmIPaddress`, you can get vmName & vmIPaddres from the portal azure

