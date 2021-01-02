#r @"./libs/Newtonsoft.Json.dll"
#r @"../../src/Farmer/bin/Debug/netstandard2.0/Farmer.dll"

open Farmer
open Farmer.Builders
open System

let myVault = keyVault { name "my-vault" }

let workspace = databricks {
    name "my-databricks-workspace"

    use_public_ip Enabled

    key_vault_secret_management myVault "workspace-encryption-key"
    key_vault_key_version Guid.Empty

    byov_vnet "databricks-vnet"
    byov_public_subnet "databricks-pub-snet"
    byov_private_subnet "databricks-priv-snet"
}

let deployment = arm {
    location Location.NorthEurope
    add_resource workspace
}

// Generate the ARM template here...
deployment
|> Writer.quickWrite "farmer-deploy"