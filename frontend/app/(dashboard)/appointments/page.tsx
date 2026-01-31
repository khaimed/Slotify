"use client";

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import api from "../../../lib/api";
import { Appointment, Client, Service } from "../../../types";
import { Calendar as CalendarIcon, Filter, Plus, Clock, User, CheckCircle2, XCircle, AlertCircle } from "lucide-react";
import { useState } from "react";
import { format } from "date-fns";
import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

function cn(...inputs: ClassValue[]) {
    return twMerge(clsx(inputs));
}

export default function AppointmentsPage() {
    const [dateFrom, setDateFrom] = useState(format(new Date(), "yyyy-MM-dd"));
    const queryClient = useQueryClient();

    const { data: appointments, isLoading } = useQuery<Appointment[]>({
        queryKey: ["appointments", dateFrom],
        queryFn: async () => {
            const { data } = await api.get("/appointments", { params: { dateFrom, dateTo: dateFrom } });
            return data;
        },
    });

    const cancelMutation = useMutation({
        mutationFn: (id: number) => api.delete(`/appointments/${id}`),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["appointments"] });
        },
    });

    return (
        <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-700">
            <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
                <div>
                    <h1 className="text-3xl font-bold text-white">Appointments</h1>
                    <p className="text-slate-400 mt-1">Schedule and manage your sessions.</p>
                </div>
                <button className="bg-blue-600 hover:bg-blue-500 text-white px-4 py-2 rounded-xl flex items-center gap-2 transition-all active:scale-95 shadow-lg shadow-blue-500/20">
                    <Plus size={18} /> New Appointment
                </button>
            </div>

            <div className="flex flex-wrap items-center gap-4 bg-slate-900/50 p-4 rounded-2xl border border-white/5">
                <div className="flex items-center gap-2 text-sm text-slate-400">
                    <Filter size={16} /> Filter:
                </div>
                <input
                    type="date"
                    className="bg-slate-800 border border-white/5 rounded-lg px-3 py-1.5 text-sm text-white focus:outline-none focus:ring-2 focus:ring-blue-500/50"
                    value={dateFrom}
                    onChange={(e) => setDateFrom(e.target.value)}
                />
            </div>

            <div className="grid grid-cols-1 gap-4">
                {isLoading ? (
                    [...Array(3)].map((_, i) => (
                        <div key={i} className="h-24 bg-slate-900/50 border border-white/5 rounded-2xl animate-pulse"></div>
                    ))
                ) : appointments?.length === 0 ? (
                    <div className="bg-slate-900/50 border border-white/5 rounded-2xl py-12 text-center text-slate-500">
                        <CalendarIcon size={48} className="mx-auto mb-4 opacity-20" />
                        <p>No appointments for this date.</p>
                    </div>
                ) : (
                    appointments?.map((app) => (
                        <div key={app.id} className="bg-slate-900/50 border border-white/5 p-5 rounded-2xl flex flex-col md:flex-row md:items-center justify-between gap-4 hover:border-blue-500/30 transition-all group">
                            <div className="flex items-start gap-4">
                                <div className={cn(
                                    "p-3 rounded-xl",
                                    app.status === "Scheduled" && "bg-blue-500/10 text-blue-400",
                                    app.status === "Completed" && "bg-emerald-500/10 text-emerald-400",
                                    app.status === "Cancelled" && "bg-red-500/10 text-red-400",
                                    app.status === "NoShow" && "bg-amber-500/10 text-amber-400",
                                )}>
                                    {app.status === "Scheduled" && <Clock size={24} />}
                                    {app.status === "Completed" && <CheckCircle2 size={24} />}
                                    {app.status === "Cancelled" && <XCircle size={24} />}
                                    {app.status === "NoShow" && <AlertCircle size={24} />}
                                </div>
                                <div>
                                    <div className="flex items-center gap-2">
                                        <h3 className="text-lg font-bold text-white">{app.clientName}</h3>
                                        <span className={cn(
                                            "text-[10px] uppercase tracking-wider font-bold px-2 py-0.5 rounded-full",
                                            app.status === "Scheduled" && "bg-blue-500/20 text-blue-400",
                                            app.status === "Completed" && "bg-emerald-500/20 text-emerald-400",
                                            app.status === "Cancelled" && "bg-red-500/20 text-red-400",
                                            app.status === "NoShow" && "bg-amber-500/20 text-amber-400",
                                        )}>
                                            {app.status}
                                        </span>
                                    </div>
                                    <p className="text-slate-300 text-sm">{app.serviceName}</p>
                                    <div className="flex items-center gap-4 mt-2 text-xs text-slate-500">
                                        <span className="flex items-center gap-1">
                                            <Clock size={12} /> {format(new Date(app.startAt), "HH:mm")} - {format(new Date(app.endAt), "HH:mm")}
                                        </span>
                                        <span className="flex items-center gap-1">
                                            <User size={12} /> {app.clientName}
                                        </span>
                                    </div>
                                </div>
                            </div>

                            <div className="flex items-center gap-2">
                                {app.status === "Scheduled" && (
                                    <button
                                        onClick={() => cancelMutation.mutate(app.id)}
                                        className="text-xs font-semibold text-red-400 bg-red-400/10 px-3 py-2 rounded-lg hover:bg-red-400/20 transition-colors"
                                    >
                                        Cancel
                                    </button>
                                )}
                                <button className="text-xs font-semibold text-slate-400 bg-slate-800 px-3 py-2 rounded-lg hover:bg-slate-700 transition-colors">
                                    Details
                                </button>
                            </div>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
}
