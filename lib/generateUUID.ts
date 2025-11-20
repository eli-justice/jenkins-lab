import { v4 as uuidv4 } from "uuid";

/**
 * Generate a unique UUID v4 string
 */
export const generateUUID = (): string => {
  return uuidv4();
};
