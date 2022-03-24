// <auto-generated>
// MIT
// </auto-generated>

namespace Eryph.ComputeClient
{
    using Eryph.ClientRuntime;
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// VirtualMachinesOperations operations.
    /// </summary>
    public partial interface IVirtualMachinesOperations
    {
        /// <summary>
        /// Get a Virtual Machines
        /// </summary>
        /// <remarks>
        /// Get a Virtual Machines
        /// </remarks>
        /// <param name='id'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Eryph.ClientRuntime.ApiServiceException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<Eryph.ClientRuntime.EryphOperationResponse<VirtualMachine>> GetWithHttpMessagesAsync(string id, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get list of Virtual Machines
        /// </summary>
        /// <remarks>
        /// Get list of Virtual Machines
        /// </remarks>
        /// <param name='count'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Eryph.ClientRuntime.ApiServiceException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        Task<Eryph.ClientRuntime.EryphOperationResponse<Eryph.ClientRuntime.IPage<VirtualMachine>>> ListWithHttpMessagesAsync(bool? count = default(bool?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get list of Virtual Machines
        /// </summary>
        /// <remarks>
        /// Get list of Virtual Machines
        /// </remarks>
        /// <param name='nextPageLink'>
        /// The NextLink from the previous successful call to List operation.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Eryph.ClientRuntime.ApiServiceException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<Eryph.ClientRuntime.EryphOperationResponse<Eryph.ClientRuntime.IPage<VirtualMachine>>> ListNextWithHttpMessagesAsync(string nextPageLink, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}