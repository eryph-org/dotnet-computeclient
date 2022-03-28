// <auto-generated>
// MIT
// </auto-generated>

namespace Eryph.ComputeClient.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class MachineConfig
    {
        /// <summary>
        /// Initializes a new instance of the MachineConfig class.
        /// </summary>
        public MachineConfig()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the MachineConfig class.
        /// </summary>
        public MachineConfig(string name = default(string), string environment = default(string), string project = default(string), VirtualMachineConfig vm = default(VirtualMachineConfig), IList<MachineNetworkConfig> networks = default(IList<MachineNetworkConfig>), MachineProvisioningConfig provisioning = default(MachineProvisioningConfig))
        {
            Name = name;
            Environment = environment;
            Project = project;
            Vm = vm;
            Networks = networks;
            Provisioning = provisioning;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "environment")]
        public string Environment { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "project")]
        public string Project { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "vm")]
        public VirtualMachineConfig Vm { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "networks")]
        public IList<MachineNetworkConfig> Networks { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "provisioning")]
        public MachineProvisioningConfig Provisioning { get; set; }

    }
}
