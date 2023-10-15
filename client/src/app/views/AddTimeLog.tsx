import React, { useState } from 'react';
import {NewTimeLog,createTimeLog} from "../api/timelogs";  

export default function TimelogsForm({  projectId, onClose }: AddTimeLogProps) {
    const [comment, setComment] = useState<string>('');
    const [logDate, setLogDate] = useState<string>(new Date().toISOString());
    const [logTimeInMinutes, setLogTimeInMinutes] = useState<number>(30); 

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (comment && logDate && logTimeInMinutes&& projectId) {
            
            const newTimeLog: NewTimeLog = { comment,  logDate: new Date(logDate), logTimeInMinutes,projectId };
            console.log(newTimeLog);
            const isResponseOk =  await createTimeLog(newTimeLog);
            console.log(isResponseOk);
            // Reset form
            setComment('');
            setLogDate(new Date().toISOString());
            setLogTimeInMinutes(0);            
            onClose();
        }
    };

    return (
        <div className="modal flex items-center my-6">
            <form id="addTimelogForm"  onSubmit={handleSubmit}>
                <div>
                    <label>
                        Comment:
                        <input value={comment} type="text" onChange={e => setComment(e.target.value)} required />
                    </label>
                </div>
                <div>
                    <label>
                        Date:
                     <input type="date" value={logDate}  onChange={(e) => setLogDate(e.target.value)}  required/>
                    </label>
                </div>
                <div>
                     <label>
                     Logged time:
                     <input value={logTimeInMinutes} type="number" onChange={e => setLogTimeInMinutes(Number(e.target.value))} min="30" required />
                </label> 
                </div>
                <div>       
                     <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"  type="submit">Save</button>        
                     <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={onClose}>Close</button>     
                </div>
            </form>
        </div>
    );
}

interface AddTimeLogProps {
    projectId: number;
    onClose: () => void;
}
