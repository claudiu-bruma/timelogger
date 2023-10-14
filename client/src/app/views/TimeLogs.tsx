import React, { useState, useEffect } from 'react';
// import Table from "../components/Table";
import {getTimelogsByProjectId,TimeLog} from "../api/timelogs";   

interface TimeLogsProps {
    projectId: number | null;
}
export default function TimeLogs({ projectId }: TimeLogsProps){
    const [timelogs, setTimelogs] = useState<TimeLog[]>([]);

    useEffect(() => {
        if (projectId !== null) {
            async function fetchTimeLogs() {
                const logs = await getTimelogsByProjectId(projectId);
                setTimelogs(logs);
            }
            fetchTimeLogs();
        }
    }, [projectId]);
    
    if (projectId === null) {
        return null; // Or return some placeholder if desired.
    }
    return (
        <>
            <table className="table-fixed w-full">
            <thead className="bg-gray-200">
                <tr>
                    <th className="border px-4 py-2 w-12">Id</th>
                    <th className="border px-4 py-2">Comment</th>
                    <th className="border px-4 py-2">Log Date</th>
                    <th className="border px-4 py-2">Minutes</th> 
                </tr>
            </thead>
            <tbody>


            {timelogs.map(timelog => (
                 <tr key = {timelog.id}>
                    <td className="border px-4 py-2 w-12">{timelog.id}</td>
                    <td className="border px-4 py-2">{timelog.comment}</td>
                    <td className="border px-4 py-2">{timelog.logDate}</td>                    
                    <td className="border px-4 py-2">{timelog.logTimeInMinutes}</td>         
                 </tr>                    
                ))} 
            </tbody>
        </table>             
        </>
    );
}
