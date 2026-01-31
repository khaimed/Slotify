"use client";

import Link from "next/link";
import { useAuth } from "../context/AuthContext";
import { Calendar, Users, ClipboardList, LogOut, User as UserIcon } from "lucide-react";

export default function Navbar() {
    const { user, logout } = useAuth();

    if (!user) return null;

    return (
        <nav className="bg-slate-900 text-white p-4 flex justify-between items-center shadow-lg">
            <div className="flex items-center gap-8">
                <Link href="/dashboard" className="text-xl font-bold bg-gradient-to-r from-blue-400 to-emerald-400 bg-clip-text text-transparent">
                    RendeVu
                </Link>
                <div className="flex gap-4">
                    <Link href="/appointments" className="flex items-center gap-2 hover:text-blue-400 transition-colors text-sm">
                        <Calendar size={18} /> Appointments
                    </Link>
                    <Link href="/clients" className="flex items-center gap-2 hover:text-blue-400 transition-colors text-sm">
                        <Users size={18} /> Clients
                    </Link>
                    {user.role === "Admin" && (
                        <Link href="/services" className="flex items-center gap-2 hover:text-blue-400 transition-colors text-sm">
                            <ClipboardList size={18} /> Services
                        </Link>
                    )}
                </div>
            </div>
            <div className="flex items-center gap-4">
                <div className="flex items-center gap-2 text-sm text-slate-300">
                    <UserIcon size={16} />
                    <span>{user.fullName} ({user.role})</span>
                </div>
                <button
                    onClick={logout}
                    className="bg-slate-800 hover:bg-red-900/50 p-2 rounded-lg transition-colors text-slate-300 hover:text-red-400"
                    title="Logout"
                >
                    <LogOut size={18} />
                </button>
            </div>
        </nav>
    );
}
