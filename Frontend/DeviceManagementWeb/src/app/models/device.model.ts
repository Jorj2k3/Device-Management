export interface Device {
  id: number;
  name: string;
  manufacturer: string;
  type: string;
  operatingSystem: string;
  osVersion: string;
  processor: string;
  ramAmountGb: number;
  description?: string;
  status: 'Available' | 'In Use' | 'Under Repair';
  assignedUserId?: number | null;
  assignedUserName?: string | null;
}