import React, { useState } from 'react';
import {createTimeLog} from "../api/timelogs";  
import { NewTimeLog } from "../models/NewTimeLog";

export default function TimelogsForm({  projectId, onClose }: AddTimeLogProps) {
    const [comment, setComment] = useState<string>('');
    const [logDate, setLogDate] = useState<string>(new Date().toISOString());
    const [logTimeInMinutes, setLogTimeInMinutes] = useState<number>(30); 
    const [errorMessage, setErrorMessage] = useState<string>('');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (comment && logDate && logTimeInMinutes&& projectId) {
            
            const newTimeLog: NewTimeLog = { comment,  logDate: new Date(logDate), logTimeInMinutes,projectId };  
            const apiResponse =  await createTimeLog(newTimeLog); 
            if(apiResponse.statusCode !== 201 && apiResponse.statusCode !== 200){
                setErrorMessage(apiResponse.message);
                return;
            } 
            
            // Reset form            
            setComment('');
            setLogDate(new Date().toISOString());
            setLogTimeInMinutes(0);            
            onClose();
        }
    };

    return ( 
        <form id="addTimelogForm" onSubmit={handleSubmit} className="space-y-4">
        <div className="flex flex-col">
            <label htmlFor="comment" className="mb-2 font-medium">Comment:</label>
            <input 
                id="comment"
                type="text"
                value={comment} 
                onChange={e => setComment(e.target.value)} 
                className="p-2 border rounded-md"
                required
            />
        </div>
    
        <div className="flex flex-col">
            <label htmlFor="logDate" className="mb-2 font-medium">Date:</label>
            <input 
                id="logDate"
                type="date" 
                value={logDate} 
                onChange={(e) => setLogDate(e.target.value)} 
                className="p-2 border rounded-md"
                required
            />
        </div>
    
        <div className="flex flex-col">
            <label htmlFor="logTime" className="mb-2 font-medium">Logged time:</label>
            <input 
                id="logTime"
                type="number" 
                value={logTimeInMinutes} 
                onChange={e => setLogTimeInMinutes(Number(e.target.value))} 
                className="p-2 border rounded-md"
                min="30"
                required
            />
        </div>
        {errorMessage && ( // Conditional rendering based on errorMessage
        <div className="mt-4 px-4 py-2 border border-red-500 bg-red-100 text-red-600 rounded">
          {errorMessage}
        </div>
      )}
        <div className="flex space-x-2">       
            <button 
               className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
                type="submit"
            >
                Save
            </button>        
            <button 
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
                onClick={onClose}
            >
                Close
            </button>     
        </div>
    </form>
    
        
    );
}

interface AddTimeLogProps {
    projectId: number;
    onClose: () => void;
}
