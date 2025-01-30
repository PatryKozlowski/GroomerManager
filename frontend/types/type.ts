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
  fullName: string;
}

export interface Client {
  id: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string | null;
}

export interface ClientDetails {
  name: string;
  phoneNumber: string;
  email: string | null;
}

export interface ClientsResponse {
  clients: Client[];
  totalCount: number;
  pageCount: number;
}

export interface ClientResponse {
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string | null;
}

export interface Note {
  id: string;
  text: string;
  created: string;
  createdBy: string;
}

export type NotesResponse = Note;

export type Notes = NotesResponse[];

export interface AddNewNote {
  // clientId: string;
  note: string;
}

export interface AddNewClientResponse {
  clientId: string;
}

export interface AddNewClient {
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string | null;
}

export interface MessageResponse {
  message: string;
}

export interface EditClient extends AddNewClient {
  id: string;
}

export type EditClientResponse = AddNewClientResponse;
