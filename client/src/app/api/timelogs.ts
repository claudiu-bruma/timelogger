const BASE_URL = "http://localhost:3001/api";
// add another function for getting a signle project
export  const getTimelogsByProjectId= async (id :number | null): Promise<TimeLog[]> => {
    const response = await fetch(`${BASE_URL}/Timelogs?projectId=${id}`);
    const data = await response.json();
    return data as TimeLog[];
}

export interface TimeLog {
    id: number;
    comment: string;
    logDate: Date;
    logTimeInMinutes: number;
    projectId: number;
}
