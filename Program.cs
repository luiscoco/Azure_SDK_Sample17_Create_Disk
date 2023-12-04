using System;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Resources;

//1. Azure Authorization
ArmClient armClient = new ArmClient(new DefaultAzureCredential());
SubscriptionResource subscription = await armClient.GetDefaultSubscriptionAsync();

//2. Get the resource group
string rgName = "myRG";
ResourceGroupResource resourceGroup = await subscription.GetResourceGroups().GetAsync(rgName);

//3. Get the disk collection from the resource group
ManagedDiskCollection diskCollection = resourceGroup.GetManagedDisks();

string diskName = "myDisk";

ManagedDiskData input = new ManagedDiskData(resourceGroup.Data.Location)
{
    Sku = new DiskSku()
    {
        Name = DiskStorageAccountType.StandardLrs
    },
    CreationData = new DiskCreationData(DiskCreateOption.Empty),
    DiskSizeGB = 177,
};

//4. Create a new disk
ArmOperation<ManagedDiskResource> lro = await diskCollection.CreateOrUpdateAsync(WaitUntil.Completed, diskName, input);
ManagedDiskResource disk = lro.Value;






