import { useState, useEffect } from "react";
import axios from "axios";
import ReactPlayer from "react-player";
import config from "../config";

const API_BASE_URL = config.API_BASE_URL;

const VideoList = () => {
  const [videos, setVideos] = useState([]);

  useEffect(() => {
    axios.get(`${API_BASE_URL}/api/videos`)
      .then((res) => setVideos(res.data))
      .catch((err) => console.error("Error fetching videos", err));
  }, []);

  return (
    <div className="mt-4">
      <h2 className="text-lg font-bold">Uploaded Videos</h2>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {videos.map((video) => (
          <div key={video.id} className="p-4 border rounded-lg shadow">
            <ReactPlayer url={video.url} controls width="100%" />
            <p className="mt-2 font-medium">{video.filename}</p>
            <img src={video.thumbnail} alt="Thumbnail" className="mt-2 w-32 h-32" />
          </div>
        ))}
      </div>
    </div>
  );
};

export default VideoList;
