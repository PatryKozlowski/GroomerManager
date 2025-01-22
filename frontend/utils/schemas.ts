import * as z from "zod";
import { toTypedSchema } from "@vee-validate/zod";

export const loginFormSchema = toTypedSchema(
  z.object({
    email: z
      .string()
      .email({
        message: "Podaj poprawny adres email",
      })
      .min(1, {
        message: "Adres email jest wymagany",
      }),
    password: z
      .string()
      .min(8, {
        message: "Hasło musi mieć minimum 8 znaków",
      })
      .regex(/(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])/, {
        message: "Hasło musi zawierać małą i dużą literę oraz cyfrę",
      })
      .regex(/[^a-zA-Z0-9]/, {
        message: "Hasło musi zawierać znak specjalny",
      }),
  })
);

const MAX_FILE_SIZE = 2 * 1024 * 1024;

export const addFirstSalonFormSchema = [
  z.object({
    name: z
      .string({ message: "Nazwa salonu jest wymagana" })
      .min(3, { message: "Nazwa salonu powinna być dłuzsza" })
      .max(25, { message: "Maksymalna dlugość nazwy to 25" }),
  }),
  z.object({
    logo: z
      .instanceof(File, { message: "Musisz wybrac logo salonu" })
      .refine((file) => file?.type === "image/png", {
        message: "Plik musi być obrazem w formacie PNG.",
      })
      .refine((file) => file instanceof File && file.size <= MAX_FILE_SIZE, {
        message: "Rozmiar pliku nie może przekraczać 2 MB",
      }),
  }),
];

export const addSalonFormSchema = toTypedSchema(
  z.object({
    name: z
      .string({ message: "Nazwa salonu jest wymagana" })
      .min(3, { message: "Nazwa salonu powinna być dłuzsza" })
      .max(25, { message: "Maksymalna dlugość nazwy to 25" }),
    logo: z
      .instanceof(File, { message: "Musisz wybrac logo salonu" })
      .refine((file) => file?.type === "image/png", {
        message: "Plik musi być obrazem w formacie PNG.",
      })
      .refine((file) => file instanceof File && file.size <= MAX_FILE_SIZE, {
        message: "Rozmiar pliku nie może przekraczać 2 MB",
      }),
  })
);
