"use client";

import { useQuery } from "@tanstack/react-query";
import api from "../../../lib/api";
import { Client } from "../../../types";
import { UserPlus, Search, Phone, Mail, MoreVertical } from "lucide-react";
import { useState } from "react";

export default function ClientsPage() {
    const [search, setSearch] = useState("");

    const { data: clients, isLoading } = useQuery<Client[]>({
        queryKey: ["clients", search],
        queryFn: async () => {
            const { data } = await api.get("/clients", { params: { search } });
            return data;
        },
    });

    return (
        <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-700">
            <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
                <div>
                    <h1 className="text-3xl font-bold text-white">Clients</h1>
                    <p className="text-slate-400 mt-1">Manage your customer database and contact info.</p>
                </div>
                <button className="bg-blue-600 hover:bg-blue-500 text-white px-4 py-2 rounded-xl flex items-center gap-2 transition-all active:scale-95 shadow-lg shadow-blue-500/20">
                    <UserPlus size={18} /> Add Client
                </button>
            </div>

            <div className="relative">
                <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-500" size={18} />
                <input
                    type="text"
                    placeholder="Search by name, email, or phone..."
                    className="w-full bg-slate-900/50 border border-white/5 rounded-xl py-3 pl-10 pr-4 text-white focus:outline-none focus:ring-2 focus:ring-blue-500/50 transition-all"
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                />
            </div>

            <div className="bg-slate-900/50 border border-white/5 rounded-2xl overflow-hidden shadow-xl">
                <table className="w-full text-left">
                    <thead className="bg-white/5 text-slate-400 text-sm font-medium">
                        <tr>
                            <th className="px-6 py-4">Full Name</th>
                            <th className="px-6 py-4">Contact</th>
                            <th className="px-6 py-4">Status</th>
                            <th className="px-6 py-4">Created</th>
                            <th className="px-6 py-4 text-right">Actions</th>
                        </tr>
                    </thead>
                    <tbody className="divide-y divide-white/5">
                        {isLoading ? (
                            [...Array(3)].map((_, i) => (
                                <tr key={i} className="animate-pulse">
                                    <td colSpan={5} className="px-6 py-8 h-16 bg-white/5 border-b border-white/5"></td>
                                </tr>
                            ))
                        ) : clients?.length === 0 ? (
                            <tr>
                                <td colSpan={5} className="px-6 py-12 text-center text-slate-500 italic">
                                    No clients found.
                                </td>
                            </tr>
                        ) : (
                            clients?.map((client) => (
                                <tr key={client.id} className="hover:bg-white/5 transition-colors group">
                                    <td className="px-6 py-4">
                                        <div className="font-medium text-white">{client.fullName}</div>
                                        <div className="text-xs text-slate-500">ID: #{client.id}</div>
                                    </td>
                                    <td className="px-6 py-4 space-y-1">
                                        <div className="flex items-center gap-2 text-sm text-slate-300">
                                            <Phone size={14} className="text-slate-500" /> {client.phone}
                                        </div>
                                        <div className="flex items-center gap-2 text-sm text-slate-300">
                                            <Mail size={14} className="text-slate-500" /> {client.email}
                                        </div>
                                    </td>
                                    <td className="px-6 py-4">
                                        <span className="bg-emerald-500/10 text-emerald-400 text-xs px-2 py-1 rounded-full font-medium">
                                            Active
                                        </span>
                                    </td>
                                    <td className="px-6 py-4 text-sm text-slate-400">
                                        {new Date(client.createdAt).toLocaleDateString()}
                                    </td>
                                    <td className="px-6 py-4 text-right">
                                        <button className="p-2 hover:bg-white/10 rounded-lg text-slate-400 transition-colors">
                                            <MoreVertical size={18} />
                                        </button>
                                    </td>
                                </tr>
                            ))
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
