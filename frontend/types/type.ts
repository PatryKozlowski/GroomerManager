export interface LoginRequestDto {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  tokenExpired: number;
  refreshToken: string;
}

export interface Salon {
  id: string;
  name: string;
  logoPath: string;
}

export type SalonsResponse = Salon[];

export interface AddSalonForm {
  name: string;
  logo: File;
}

export interface AddSalonResponse {
  salonId: string;
  logoPath: string;
  name: string;
}

export type Role = "Owner" | "Employee";

export interface LoggedInUser {
  id: string;
  email: string;
  role: Role;
  firstName: string;
  lastName: string;
  initials: string;
}

export interface Client {
  id: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string | null;
}

export interface ClientsResponse {
  clients: Client[];
  totalCount: number;
  pageCount: number;
}

export interface ClientResponse {
  name: string;
  phone: string;
  email: string | null;
  notes: NotesResponse[];
}

export interface NotesResponse {
  id: number;
  text: string;
  createdBy: string;
  created: string;
}

export interface AddNewNote {
  clientId: number;
  text: string;
}

export type AddNoteForm = Pick<AddNewNote, "text">;

export interface AddNewClientResponse {
  clientId: number;
}

export interface AddNewClient {
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string | null;
}

export type EditClient = AddNewClient;
