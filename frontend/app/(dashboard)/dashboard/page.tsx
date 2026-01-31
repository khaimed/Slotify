"use client";

import { useAuth } from "../../../context/AuthContext";
import { Calendar, Users, ClipboardList, TrendingUp } from "lucide-react";

export default function DashboardPage() {
    const { user } = useAuth();

    return (
        <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
            <div>
                <h1 className="text-3xl font-bold text-white">Dashboard</h1>
                <p className="text-slate-400 mt-1">Welcome back, {user?.fullName}. Here's what's happening today.</p>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <StatCard
                    title="Total Appointments"
                    value="12"
                    icon={<Calendar className="text-blue-400" />}
                    trend="+20% from last week"
                />
                <StatCard
                    title="Active Clients"
                    value="48"
                    icon={<Users className="text-emerald-400" />}
                    trend="+5 new this month"
                />
                <StatCard
                    title="Services"
                    value="6"
                    icon={<ClipboardList className="text-purple-400" />}
                    trend="All systems active"
                />
                <StatCard
                    title="Monthly Revenue"
                    value="$2,400"
                    icon={<TrendingUp className="text-amber-400" />}
                    trend="+12% growth"
                />
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
                <div className="lg:col-span-2 bg-slate-900/50 border border-white/5 rounded-2xl p-6 h-96 flex items-center justify-center">
                    <p className="text-slate-500 italic">Calendar View Placeholder - Implementation coming soon</p>
                </div>
                <div className="bg-slate-900/50 border border-white/5 rounded-2xl p-6 h-96">
                    <h3 className="text-lg font-semibold mb-4">Upcoming Appointments</h3>
                    <div className="space-y-4">
                        <p className="text-slate-500 text-sm">No appointments scheduled for today.</p>
                    </div>
                </div>
            </div>
        </div>
    );
}

function StatCard({ title, value, icon, trend }: { title: string; value: string; icon: React.ReactNode; trend: string }) {
    return (
        <div className="bg-slate-900/50 border border-white/5 p-6 rounded-2xl hover:border-blue-500/30 transition-colors group">
            <div className="flex justify-between items-start mb-4">
                <div className="p-3 bg-slate-800 rounded-xl group-hover:scale-110 transition-transform">
                    {icon}
                </div>
                <span className="text-xs font-medium text-emerald-400 bg-emerald-400/10 px-2 py-1 rounded-full">
                    {trend}
                </span>
            </div>
            <div>
                <p className="text-slate-400 text-sm mb-1">{title}</p>
                <p className="text-3xl font-bold text-white">{value}</p>
            </div>
        </div>
    );
}
