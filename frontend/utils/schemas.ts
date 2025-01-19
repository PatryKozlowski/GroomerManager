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
