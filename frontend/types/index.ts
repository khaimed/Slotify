export type Role = "Admin" | "User";

export interface User {
    id: number;
    fullName: string;
    email: string;
    role: Role;
    token: string;
}

export interface Client {
    id: number;
    fullName: string;
    phone: string;
    email: string;
    notes?: string;
    createdAt: string;
}

export interface Service {
    id: number;
    name: string;
    durationMinutes: number;
    price?: number;
    description?: string;
    isActive: boolean;
}

export type AppointmentStatus = "Scheduled" | "Cancelled" | "Completed" | "NoShow";

export interface Appointment {
    id: number;
    clientId: number;
    clientName: string;
    serviceId: number;
    serviceName: string;
    startAt: string;
    endAt: string;
    status: AppointmentStatus;
    notes?: string;
    createdByUserId: number;
}
