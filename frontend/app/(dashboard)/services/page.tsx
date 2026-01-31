"use client";

import { useQuery } from "@tanstack/react-query";
import api from "../../../lib/api";
import { Service } from "../../../types";
import { Package, Plus, DollarSign, Clock, ToggleLeft, ToggleRight } from "lucide-react";

export default function ServicesPage() {
    const { data: services, isLoading } = useQuery<Service[]>({
        queryKey: ["services"],
        queryFn: async () => {
            const { data } = await api.get("/services");
            return data;
        },
    });

    return (
        <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-700">
            <div className="flex justify-between items-center">
                <div>
                    <h1 className="text-3xl font-bold text-white">Services</h1>
                    <p className="text-slate-400 mt-1">Define what you offer and for how long.</p>
                </div>
                <button className="bg-purple-600 hover:bg-purple-500 text-white px-4 py-2 rounded-xl flex items-center gap-2 transition-all active:scale-95 shadow-lg shadow-purple-500/20">
                    <Plus size={18} /> Create Service
                </button>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-6">
                {isLoading ? (
                    [...Array(3)].map((_, i) => (
                        <div key={i} className="h-48 bg-slate-900/50 border border-white/5 rounded-2xl animate-pulse"></div>
                    ))
                ) : services?.length === 0 ? (
                    <div className="col-span-full py-20 text-center text-slate-500">
                        <Package size={48} className="mx-auto mb-4 opacity-20" />
                        <p>No services defined yet.</p>
                    </div>
                ) : (
                    services?.map((service) => (
                        <div key={service.id} className={`bg-slate-900/50 border border-white/5 p-6 rounded-2xl hover:border-purple-500/30 transition-all group relative overflow-hidden ${!service.isActive && 'opacity-60'}`}>
                            <div className="flex justify-between items-start mb-4">
                                <div className="p-3 bg-slate-800 rounded-xl group-hover:bg-purple-500/20 transition-colors">
                                    <Package className="text-purple-400" size={24} />
                                </div>
                                <button className="text-slate-500 hover:text-white transition-colors">
                                    {service.isActive ? <ToggleRight className="text-emerald-400" size={32} /> : <ToggleLeft size={32} />}
                                </button>
                            </div>

                            <h3 className="text-xl font-bold text-white mb-2">{service.name}</h3>
                            <p className="text-slate-400 text-sm line-clamp-2 mb-6 h-10">{service.description || "No description provided."}</p>

                            <div className="flex items-center gap-6 border-t border-white/5 pt-4 text-sm font-medium">
                                <div className="flex items-center gap-2 text-slate-300">
                                    <Clock size={16} className="text-purple-400" />
                                    {service.durationMinutes} min
                                </div>
                                {service.price && (
                                    <div className="flex items-center gap-2 text-slate-300">
                                        <DollarSign size={16} className="text-emerald-400" />
                                        {service.price}
                                    </div>
                                )}
                            </div>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
}
