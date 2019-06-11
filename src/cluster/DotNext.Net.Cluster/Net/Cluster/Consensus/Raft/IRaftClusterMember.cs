﻿using System;
using System.Threading;
using System.Threading.Tasks;
using DotNext.Net.Cluster.Replication;

namespace DotNext.Net.Cluster.Consensus.Raft
{
    /// <summary>
    /// Represents cluster member accessible through Raft protocol.
    /// </summary>
    public interface IRaftClusterMember : IClusterMember, IDisposable
    {
        /// <summary>
        /// Requests vote from the member.
        /// </summary>
        /// <param name="lastEntry">The identifier of the last log entry. May be <see langword="null"/> if audit trail is not established for the member.</param>
        /// <param name="token">The token that can be used to cancel asynchronous operation.</param>
        /// <returns>Vote received from member; <see langword="true"/> if node accepts new leader, <see langword="false"/> if node doesn't accept new leader, <see langword="null"/> if node is not available.</returns>
        Task<bool?> VoteAsync(LogEntryId? lastEntry, CancellationToken token);

        /// <summary>
        /// Revokes leadership.
        /// </summary>
        /// <param name="token">The token that can be used to cancel asynchronous operation.</param>
        /// <returns><see langword="true"/>, if leadership is revoked successfully; otherwise, <see langword="false"/>.</returns>
        Task<bool> ResignAsync(CancellationToken token);

        Task HeartbeatAsync(CancellationToken token);

        /// <summary>
        /// Transfers transaction log entry to the member.
        /// </summary>
        /// <param name="newEntry">The message representing set of changes. Can be <see langword="null"/>.</param>
        /// <param name="lastEntry">The identifier of the last log entry stored in the caller node.</param>
        /// <param name="token">The token that can be used to cancel asynchronous operation.</param>
        /// <returns><see langword="true"/> if message is handled successfully by this member; <see langword="false"/> if message is rejected due to invalid Term/Index number.</returns>
        /// <exception cref="MemberUnavailableException">The member is unreachable through network.</exception>
        Task<bool> AppendEntriesAsync(ILogEntry newEntry, LogEntryId lastEntry, CancellationToken token);

        /// <summary>
        /// Aborts all active outbound requests.
        /// </summary>
        void CancelPendingRequests();
        long LastIndex { get; set; }
    }
}